namespace Graduation_Project.Application.Utils {

    public class RegularExpressions {
        public const string EmailPattern = @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}$";
        public const string EmailOrUserNamePattern = @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*$";
    }
}