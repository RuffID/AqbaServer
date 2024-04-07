using System.Text.RegularExpressions;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Helper
{
    internal static partial class ReportDataHandler
    {
        // Шаблон Фамилии + имени для страницы с заявками, парсятся несколько символов впереди чтобы получить именно ФИО, а не "Решено заявок" например
        // Пример строки: <span>Карпенко Алексей Сергеевич - для страницы с решёнными заявками
        [GeneratedRegex(@"\S{1}[a-z]{4}\S{1}([А-ЯЁ]+[а-яё]+)\s+([А-ЯЁ]+[a-яё]+|[А-ЯЁ]+|[а-яё]+)")]
        private static partial Regex EmployeeTaskRegex();

        // Пример строки: 52\"\u003eКарпенко Алексей Сергеевич - для страницы со списанным временем
        [GeneratedRegex(@"\d+\S{3}[a-z]{1}\d{3}[a-z]{1}([А-ЯЁ]+[а-яё]+)\s+([А-ЯЁ]+[a-яё]+|[А-ЯЁ]+|[а-яё]+)")]
        private static partial Regex EmployeeTimeRegex();

        // Шаблон текста перед количеством заявок и само количество
        // Пример строки: UTC&amp;q%5Bstatus_id_eq_any%5D%5B%5D=6285&amp;q%5Bstatus_id_eq_any%5D%5B%5D=6284\"><span>2754
        [GeneratedRegex(@"\D+\S{81}\W{1}[a-z]{4}\W{1}(\d+)")]
        private static partial Regex NumberOfSolvedTasksRegex();

        // Шаблон часов 0 ч. 0 м. - берутся только значения (цифры) без лишних символов, т.к. найти значение легко и его не спутать
        // Пример строки: 2956 ч. 59 м.
        [GeneratedRegex(@"(\d+)\s+\w+\S+\s+(\d+)\s+\w+.")]
        private static partial Regex AmountOfTimeWrittenOfRegex();

        // Шаблон текущей страницы. На первой странице с решёнными заявками снизу выводятся страницы, шаблон для этого списка страниц, 
        // Пример строки: completed-count_desc\">2
        [GeneratedRegex(@"[a-z]{4}\s{1}[a-z]{7}\S{3}(\d{1})")]
        private static partial Regex TaskCurrentPageRegex();

        // Шаблон текущей страницы на странице со списанным временем
        // Пример строки: page current\"\u003e1
        [GeneratedRegex(@"[a-z]{4}\s{1}[a-z]{7}\S{3}[a-z]{1}\d{3}[a-z]{1}(\d{1})")]
        private static partial Regex TimeCurrentPageRegex();

        // Шаблон количества страниц на странице с решёнными заявками
        // Пример строки: completed-count_desc\">2<\/a
        [GeneratedRegex(@"[a-z]{9}.[a-z]{5}.[a-z]{4}\S{3}(\d{1})\S{3}[a-z]{1}")]
        private static partial Regex PagesCountPerReportTaskPageRegex();

        // Шаблон количества страниц на странице со списанным временем
        // Пример строки: employee_time_entries.json?page=2
        [GeneratedRegex(@"[a-z]{8}.[a-z]{4}.[a-z]{7}.[a-z]{4}.[a-z]{4}.(\d{1})")]
        private static partial Regex PagesCountPerReportTimePageRegex();

        static MatchCollection? matchesEmployees;    // Найденные сотрудники на странице с отчётом
        static MatchCollection? matchesValues;   // Найденные значение решённых заявок/списанного времени на странице с отчётом
        static readonly Match? currentTaskPage;  // Номер текущей страницы на странице с решёнными заявками
        static readonly Match? currentTimePage;  // Номер текущей страницы на странице со списанным временем
        static MatchCollection? matchesPagesTaskCount;   // Число страниц с решёнными заявками
        static MatchCollection? matchesPagesTimeCount;   // Число страниц со списанным временем

        public static void GetEmployeesResults(string? rawData, List<Employee> employees)  // Вызывает метод по поиску данных со странички
        {
            if (string.IsNullOrEmpty(rawData))
            {
                WriteLog.Warn($"Получена пустая страница данных по заявкам либо задачам с окдеска; null or empty string rawData: {rawData}");
                return;
            }

            if (NumberOfSolvedTasksRegex().IsMatch(rawData))  // Если на страничке есть значения с задачами, то
            {
                FindMatches(EmployeeTaskRegex(), NumberOfSolvedTasksRegex(), rawData); // Матчит список сотрудников по порядку
                SaveReportResults(rawData, employees);   // Сохраняет найденные данные                
            }
            else if (AmountOfTimeWrittenOfRegex().IsMatch(rawData))   // Если на страничке есть значения времени, то
            {
                FindMatches(EmployeeTimeRegex(), AmountOfTimeWrittenOfRegex(), rawData);   // Матчит значения решённых заявок или списанных часов
                SaveReportResults(rawData, employees);   // Сохраняет найденные данные                
            }
        }

        static void SaveReportResults(string rawData, List<Employee> employees) // Поиск и запись данных в класс
        {
            if (matchesEmployees == null || matchesEmployees.Count > 0) return;  // Если не найден ни один сотрудник, то завершает метод

            for (int i = 0; i < matchesEmployees.Count; i++)    // Проход по всем сотрудником и запись данных
            {
                try
                {
                    List<Employee>? employeesWithTheSameLastName;    // Список найденных сотрудников
                                                                     // Находит всех сотрудников с совпадением по фамилии и записывает в list
                    employeesWithTheSameLastName = employees?.FindAll(x => x.Last_name.Contains(matchesEmployees[i].Groups[1].ToString()));

                    for (int j = 0; j < employeesWithTheSameLastName?.Count; j++)    // Проходит по списку найденных сотрудников по фамилии
                    {
                        // Сравнивает имя и если оно совпадает со списком в базе, то сохраняет значение,
                        // нужно для моментов, когда есть сотрудники с похожими фамилиями
                        Employee? currentEmployee;
                        // Находит по совпадению в имени чтобы не было ошибочных присваиваний и записывает ссылку на сотрудника
                        currentEmployee = employeesWithTheSameLastName.Find(x => x.First_name.Contains(matchesEmployees[i].Groups[2].ToString()));

                        if (currentEmployee == null) return;

                        if (matchesValues == null || matchesValues.Count == 0) continue;
                        // Если на страничке есть совпадения по решённым задачам, то записывает их в сотрудника
                        if (NumberOfSolvedTasksRegex().IsMatch(matchesValues[i].ToString()))
                        {
                            // Если эта первая страница, то сохраняет количество страниц в отчёте
                            SaveCountPage(currentTaskPage, TaskCurrentPageRegex(), PagesCountPerReportTaskPageRegex(), ref matchesPagesTaskCount, rawData);
                            currentEmployee.SolvedTasks = Convert.ToInt32(matchesValues[i].Groups[1].ToString());
                            break;
                        }
                        // Если на страничке есть совпадения по списанному времени, то записывает их в сотрудника
                        else if (AmountOfTimeWrittenOfRegex().IsMatch(matchesValues[i].ToString()))
                        {
                            SaveCountPage(currentTimePage, TimeCurrentPageRegex(), PagesCountPerReportTimePageRegex(), ref matchesPagesTimeCount, rawData);

                            if (matchesValues?.Count < i || matchesValues?[i] == null) break;
                            string tempH = string.Empty;
                            string tempM = string.Empty;

                            if (matchesValues[i].Groups.Count > 0)
                                tempH = matchesValues[i].Groups[1].ToString();

                            if (matchesValues[i].Groups.Count > 1)
                                tempM = matchesValues[i].Groups[2].ToString();

                            int spentedHours = 0;
                            int spentedMinutes = 0;

                            if (!string.IsNullOrEmpty(tempH))
                                spentedHours = Convert.ToInt32(tempH);

                            if (!string.IsNullOrEmpty(tempM))
                                spentedMinutes = Convert.ToInt32(tempM);

                            if (spentedHours > 0 || spentedMinutes > 0)
                                currentEmployee.SpentedTimeDouble = spentedHours + ((double)spentedMinutes / 60);

                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    WriteLog.Error(e.ToString());
                }
            }
        }

        static void SaveCountPage(Match currentPage, Regex regexPageCurrent, Regex regexPagesCountPerReportPage, ref MatchCollection matchesPagesCount, string rawData)
        {
            if (currentPage == null)    // Записывает количество страниц только один раз
            {
                currentPage = Regex.Match(rawData, regexPageCurrent.ToString());
                if (currentPage.Groups[1].ToString() == "1")    // Если это первая страница, то записывает значение
                    matchesPagesCount = Regex.Matches(rawData, regexPagesCountPerReportPage.ToString());
            }
        }

        static void FindMatches(Regex regexMatchNameTaskOrTime, Regex regexMatchesValue, string rawData)
        {
            // Поиск значений происходит для каждой странички по новому т.к. нужны совпадения по индексу сотрудник - значение
            matchesEmployees = Regex.Matches(rawData, regexMatchNameTaskOrTime.ToString()); // Матч всех сотрудников со странички
            matchesValues = Regex.Matches(rawData, regexMatchesValue.ToString());    // Матч время или кол-во решённых заявок
        }

        public static int GetPagesCountTimePerReportPage()  // Количество страниц с отчёта по списанному времени
        {
            try  // В список номеров страниц дублится следующая страница на два раза, поэтому количество - 2
            {
                if (matchesPagesTimeCount?.Count > 0)
                    return Convert.ToInt32(matchesPagesTimeCount[matchesPagesTimeCount.Count - 2].Groups[1].ToString());
                else return 1;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return 1;
            }
        }

        public static int GetPagesCountTaskPerReportPage()  // Возвращает количество страниц с отчёта по заявкам чтобы по ним можно было пройтись
        {
            try
            {
                if (matchesPagesTaskCount?.Count > 0)
                    return Convert.ToInt32(matchesPagesTaskCount[matchesPagesTaskCount.Count - 1].Groups[1].ToString());
                else return 1;
            }
            catch (Exception e)  // Если страница одна, то возвращает 1
            {
                WriteLog.Error(e.ToString());
                return 1;
            }
        }

        
    }
}
