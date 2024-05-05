using AqbaServer.Models.OkdeskPerformance;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using AqbaServer.Helper;
using AqbaServer.Models.OkdeskReport;
using AqbaServer.Dto;
using System.ComponentModel.Design;

namespace AqbaServer.API
{
    public static class Request
    {
        static readonly SocketsHttpHandler socketsHandler = new()
        {
            PooledConnectionLifetime = TimeSpan.FromSeconds(30),
        };
        static readonly HttpClient httpClient = new(socketsHandler);

        static readonly HttpClientHandler handler;  // здесь хранятся куки
        static readonly CookieContainer cookie;

        static Request()    // Конструктор класса для создания контейнера под куки при инициализации экземпляра класса
        {
            cookie = new CookieContainer();
            handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None,
                CookieContainer = cookie,
            };
        }

        public static async Task<Company[]?> GetСompanies(int lastCompanyId, int companyCategoryId, int pageSize = 100)
        {
            string link = $"{Immutable.OkdeskApiLink}/companies/list?api_token={Config.OkdeskApiToken}&page[from_id]={lastCompanyId}&page[direction]=forward&category_ids[]={companyCategoryId}&page[size]={pageSize}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Company[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
        }

        public static async Task<Company?> GetСompany(int companyId)
        {
            string link = $"{Immutable.OkdeskApiLink}/companies?api_token={Config.OkdeskApiToken}&id={companyId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Company>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
        }

        public static async Task<MaintenanceEntity[]?> GetMaintenanceEntities(int lastMaintenanceEntitiesId, int pageSize = 100, int companyId = 0)
        {
            string link = $"{Immutable.OkdeskApiLink}/maintenance_entities/list?api_token={Config.OkdeskApiToken}&page[from_id]={lastMaintenanceEntitiesId}&page[direction]=forward&page[size]={pageSize}";

            if (companyId > 0) link += $"&company_ids[]={companyId}";

            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]") return null;

            try
            {
                return JsonConvert.DeserializeObject<MaintenanceEntity[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }
        }

        public static async Task<Equipment[]?> GetEquipments(int lastEquipmentId, int pageSize = 100, int maintenanceEntityId = 0, int companyId = 0)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/list?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastEquipmentId}&page[size]={pageSize}";

            if (companyId > 0) link += $"&company_ids[]={companyId}";

            if (maintenanceEntityId > 0) link += $"&maintenance_entity_ids[]={maintenanceEntityId}";

            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]") return null;

            try
            {
                return JsonConvert.DeserializeObject<Equipment[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }
        }

        public static async Task<KindParameter[]?> GetKindParameters()
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/parameters?api_token={Config.OkdeskApiToken}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<KindParameter[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Manufacturer[]?> GetManufacturers(long lastManufacturerId)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/manufacturers?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastManufacturerId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Manufacturer[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Kind[]?> GetKinds(long lastKindId)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/kinds?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastKindId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Kind[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Model[]?> GetModels(long lastModelId)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/models?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastModelId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Model[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
        }

        public static async Task<Employee[]?> GetEmployees(long lastEmployeeId)  // Получение списка сотрудников
        {
            string link = $"{Immutable.OkdeskApiLink}/employees/list?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastEmployeeId}";
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Employee[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }
        }

        public static async Task<ICollection<Issue>?> GetUpdatedIssues(DateTime updatedSince, DateTime updatedUntil, int assigneeId)
        {
            ICollection<Issue> issues = [];
            StringBuilder linkOpenTasks = new();
            int pageNumber = 1;

            while (true)
            {
                linkOpenTasks.Clear();
                linkOpenTasks.Append($"{Immutable.OkdeskApiLink}/issues/list?api_token={Config.OkdeskApiToken}");

                linkOpenTasks.Append($"&updated_since={updatedSince.ToString("dd-MM-yyyy HH:mm:ss")}");
                linkOpenTasks.Append($"&updated_until={updatedUntil.ToString("dd-MM-yyyy HH:mm:ss")}");
                linkOpenTasks.Append($"&assignee_ids[]={assigneeId}");
                linkOpenTasks.Append("&page[size]=50");
                linkOpenTasks.Append($"&page[number]={pageNumber}");

                var responseOpen = await SendApiRequest(linkOpenTasks.ToString());

                if (!string.IsNullOrEmpty(responseOpen) && responseOpen != "[]")
                {
                    try
                    {
                        Issue[]? list = JsonConvert.DeserializeObject<Issue[]>(responseOpen);

                        if (list != null || list?.Length > 0)
                        {
                            foreach (var issue in list)
                            {
                                // Эта строчка необходима чтобы можно было делать выборку из БД по "свежим задачам" т.к. в API отсутствует employees_updated_at параметр
                                issue.Employees_updated_at = DateTime.Now;
                                // И параметр assignee_id тоже отсутствует, поэтому присваиваем заявке его тут
                                issue.Assignee_id = assigneeId;
                                issues.Add(issue);
                            }

                            pageNumber++;

                            if (list?.Length < 50)
                                break;
                        }
                        else break;
                    }
                    catch (Exception e) { WriteLog.Error(e.ToString()); break; }
                }
                else break;
            }

            return issues;

            /*StringBuilder linkOpenTasks = new();
            int pageNumber;

            foreach (var employee in employees.Where(e => e.Active))
            {
                employee.Issues = [];
                pageNumber = 1;
                while (true)
                {
                    linkOpenTasks.Clear();
                    linkOpenTasks.Append($"{Immutable.OkdeskApiLink}/issues/list?api_token={Config.OkdeskApiToken}");

                    linkOpenTasks.Append($"&assignee_ids[]={employee.Id}");
                    linkOpenTasks.Append($"&page[number]={pageNumber}");
                    linkOpenTasks.Append("&page[size]=50");
                    linkOpenTasks.Append("&status_codes_not[]=completed"); // Код открытых заявок
                    linkOpenTasks.Append("&status_codes_not[]=closed");   // Принятые заявки


                    var responseOpen = await SendApiRequest(linkOpenTasks.ToString());
                    if (!string.IsNullOrEmpty(responseOpen) && responseOpen != "[]")
                    {
                        try
                        {
                            Issue[]? list = JsonConvert.DeserializeObject<Issue[]>(responseOpen);                            

                            if (list != null || list?.Length > 0)
                            {
                                employee.Issues.AddRange( list );
                                pageNumber++;

                                if (list?.Length < 50)
                                    break;
                            }                            
                        }
                        catch (Exception e) { WriteLog.Error(e.ToString()); break; }
                    }
                    else break;
                }
            }
            return null;*/
        }

        public static async Task<IssueJSON?> GetIssue(int issueId)
        {
            string link = $"{Immutable.OkdeskApiLink}/issues/{issueId}?api_token={Config.OkdeskApiToken}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<IssueJSON>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
        }

        public static async Task<Group[]?> GetGroups()
        {
            string link = Immutable.OkdeskApiLink + "/employees/groups?api_token=" + Config.OkdeskApiToken;
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                return JsonConvert.DeserializeObject<Group[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }

        }

        public static async Task<Role[]?> GetRoles()
        {
            string link = Immutable.OkdeskApiLink + "/employees/roles?api_token=" + Config.OkdeskApiToken;
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Role[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }

        }

        public static async Task<ICollection<IssueType>?> GetTypes()
        {
            string link = Immutable.OkdeskApiLink + "/dictionaries/issues/types?api_token=" + Config.OkdeskApiToken;
            IssueType[]? types = [];
            List<IssueType> list = [];
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return list;

            try
            {
                types = JsonConvert.DeserializeObject<IssueType[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); }

            if (types != null && types.Length > 0)
            {
                list = types.ToList();
                // Цикл вытаскивает все типы из "папок" в общий список
                if (types?.Length > 0)
                {
                    foreach (var type in types)
                    {
                        if (type?.Children?.Length > 0)
                        {
                            foreach (var child in type.Children)
                            {
                                list.Add(new(child));
                            }
                            list.Remove(type);
                        }
                    }
                }
                return list;
            }
            return list;
        }

        public static async Task<Status[]?> GetStatuses()
        {
            string link = Immutable.OkdeskApiLink + "/issues/statuses?api_token=" + Config.OkdeskApiToken;
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Status[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }
        }

        public static async Task<Priority[]?> GetPriorities()
        {
            string link = Immutable.OkdeskApiLink + "/issues/priorities?api_token=" + Config.OkdeskApiToken;
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Priority[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }
        }

        public static async Task<TimeEntries?> GetTimeEntries(int issueId)
        {
            string link = Immutable.OkdeskApiLink + $"/issues/{issueId}/time_entries?api_token=" + Config.OkdeskApiToken;
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                return JsonConvert.DeserializeObject<TimeEntries>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); return null; }

        }

        /*public static async Task GetTime(ICollection<Employee> employees, DateTime dateFrom, DateTime dateTo)
        {
            foreach (var employee in employees)
            {
                employee.SpentedTimeDouble = 0;
            }

            string linkFirstPage = $"{Config.OkdeskDomainLink}/reports/employee_time_entries?reports[logged_at_from]={dateFrom:dd.MM.yyyy}&reports[logged_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=time_desc";

            string linkAnotherPage;
            var reportPage = await SendGetRequest(linkFirstPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.JSON_GET_ACCEPT_HEADER);
            ReportDataHandler.GetEmployeesResults(reportPage, employees);


            for (int i = 2; ;)
            {

                if (i <= ReportDataHandler.GetPagesCountTimePerReportPage())    // Парсит страницу со списанным временем
                {
                    linkAnotherPage = $"{Config.OkdeskDomainLink}/reports/employee_time_entries?page={i}&reports[logged_at_from]={dateFrom:dd.MM.yyyy}&reports[logged_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=time_desc";

                    var response = await SendGetRequest(linkAnotherPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.JSON_GET_ACCEPT_HEADER);
                    ReportDataHandler.GetEmployeesResults(response, employees);
                }

                // Если оба инетатора больше чем количество страниц, то завершает цикл
                if (i > ReportDataHandler.GetPagesCountTimePerReportPage())
                    break;
                
                i++;
            }
        }

        public static async Task GetTasks(ICollection<Employee> Employees, DateTime dateFrom, DateTime dateTo)
        {
            foreach (var employee in Employees)
            {
                employee.SolvedTasks = 0;
            }
            string linkFirstPage = $"{Config.OkdeskDomainLink}/reports/assignee?reports[completed_at_from]={dateFrom:dd.MM.yyyy}&reports[completed_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=completed-count_desc";

            string linkAnotherPage;
            var reportPage = await SendGetRequest(linkFirstPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.TEXT_JS_GET_ACCEPT_HEADER);
            ReportDataHandler.GetEmployeesResults(reportPage, Employees);


            for (int i = 2; ;)
            {

                if (i <= ReportDataHandler.GetPagesCountTaskPerReportPage())    // Парсит страницу с решёнными заявками
                {
                    linkAnotherPage = $"{Config.OkdeskDomainLink}/reports/assignee?page={i}&reports[completed_at_from]={dateFrom:dd.MM.yyyy}&reports[completed_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=completed-count_desc";

                    var response = await SendGetRequest(linkAnotherPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.TEXT_JS_GET_ACCEPT_HEADER);
                    ReportDataHandler.GetEmployeesResults(response, Employees);
                }

                // Если оба инетатора больше чем количество страниц, то завершает цикл
                if (i > ReportDataHandler.GetPagesCountTaskPerReportPage())
                    break;

                i++;
            }
        }*/

        public static async Task<bool> Login(string loginReferer, string content)
        {

            using var client = new HttpClient(handler, false);
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.58");
            client.DefaultRequestHeaders.Referrer = new Uri(loginReferer);
            client.DefaultRequestHeaders.Add("sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Microsoft Edge\";v=\"114");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            try
            {
                using var resp = await client.PostAsync(loginReferer, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
                IEnumerable<string> str = resp.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                if (resp.StatusCode == HttpStatusCode.Found && str.Any(str => str.Contains("remember_user_token")))
                {
                    WriteLog.Info($"Successfull login in service: {loginReferer}");
                    return true;
                }
                else
                {
                    WriteLog.Warn($"Failed to log in to the service using the link: {loginReferer}");
                    return false;
                }
            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return false;
            }
        }

        public static async Task<string?> SendGetRequest(string getReferer, string siteReferer, string acceptHeader)
        {
            using var client = new HttpClient(handler, false);
            client.DefaultRequestHeaders.Add("Accept", $"{acceptHeader}, */*; q=0.01");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Referrer = new Uri(siteReferer);
            client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"Google Chrome\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");

            try
            {
                using var resp = await client.GetAsync(getReferer);
                await Task.Delay(200);
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsStringAsync();
                else
                {
                    WriteLog.Warn($"Error when retrieving a get page using a link request: {siteReferer}");
                    return null;
                }
            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return null;
            }
        }

        public static async Task<string?> SendApiRequest(string? link)
        {
            try
            {
                await Task.Delay(300);
                using HttpResponseMessage response = await httpClient.GetAsync(link);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return null;
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                return await response.Content.ReadAsStringAsync();

            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return null;
            }
        }
    }
}