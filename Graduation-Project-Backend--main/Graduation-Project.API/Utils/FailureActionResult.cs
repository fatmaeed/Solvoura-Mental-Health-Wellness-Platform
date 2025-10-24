using Graduation_Project.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Utils {

    public static class FailureIActionResult {

        public static IActionResult FailureHandler(Failure failure) {
            return failure switch {
                NotFoundFailure _ => new NotFoundObjectResult(
                                        new ProblemDetails {
                                            Title = failure.Message,
                                            Status = 404,
                                            Detail = failure.Details
                                        }
                                        ),
                BadRequestFailure _ => new BadRequestResult(),
                UnauthorizedFailure _ => new UnauthorizedObjectResult(
                    new ProblemDetails {
                        Title = failure.Message,
                        Status = 401,
                        Detail = failure.Details
                    }

                    ),
                ForbiddenFailure _ => new ForbidResult(),
                ServerFailure _ => new StatusCodeResult(500),
                _ => new BadRequestObjectResult(
                    new ProblemDetails {
                        Title = failure.Message,
                        Status = 400,
                        Detail = failure.Details
                    }
                    ),
            };
        }
    }
}