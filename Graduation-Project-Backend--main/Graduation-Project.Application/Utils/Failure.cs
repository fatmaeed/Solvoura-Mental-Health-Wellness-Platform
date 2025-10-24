namespace Graduation_Project.Application.Utils {

    public class Failure(string message) {
        public string Message { get; set; } = message;
        public string? Details { get; set; }

        public Failure(string message, string details) : this(message) => Details = details;
    }

    public class NotFoundFailure : Failure {

        public NotFoundFailure(string message) : base(message) {
        }

        public NotFoundFailure(string message, string details) : base(message, details) {
        }
    }

    public class BadRequestFailure : Failure {

        public BadRequestFailure(string message) : base(message) {
        }

        public BadRequestFailure(string message, string details) : base(message, details) {
        }
    }

    public class UnauthorizedFailure : Failure {

        public UnauthorizedFailure(string message) : base(message) {
        }

        public UnauthorizedFailure(string message, string details) : base(message, details) {
        }
    }

    public class ForbiddenFailure : Failure {

        public ForbiddenFailure(string message) : base(message) {
        }

        public ForbiddenFailure(string message, string details) : base(message, details) {
        }
    }

    public class ServerFailure : Failure {

        public ServerFailure(string message) : base(message) {
        }

        public ServerFailure(string message, string details) : base(message, details) {
        }
    }
}