using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace NancyIntro
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Func<Request, bool> _isNotApiClient = request => !request.Headers.UserAgent.ToLower().StartsWith("curl");

            Get["/", ctx=> _isNotApiClient.Invoke(ctx.Request)] = p => View["/assets/index.html"];

            Get["/"] = p => "Welcome to the Pluralsight API!";

			/**** Course Routes ****
			 * 
			 * GET list of courses
			 * GET single course
			 * POST single course
			 * Return responses as JSON
			 * 
			 * ********************/

			Get["/courses"] = p => new JsonResponse(Repository.Courses, new DefaultJsonSerializer());

			Get["/courses/{id}"] = p => Response.AsJson((Course)Repository.GetCourse(p.id));

			Post["/courses", c => c.Request.Headers.ContentType != "application/x-www-urlencoded"] = p =>
			{
				var course = this.Bind<Course>();
				Repository.AddCourse(course);
				return NewCourseResponse(course);
			};
			Post["/courses"] = p =>
			{
				var name = this.Request.Form.Name;
				var author = this.Request.Form.Author;
				var course = Repository.AddCourse(name, author);

				return NewCourseResponse(course);
			};
		}

		Response NewCourseResponse(dynamic course)
		{
			string url = string.Format("{0}/{1}", this.Context.Request.Url, course.Id);

			return new Response()
			{
				StatusCode = HttpStatusCode.Accepted
			}
				.WithHeader("Location", url);
		}
    }
}