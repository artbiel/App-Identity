namespace Identity.Api.Contracts.v1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public static class V1
        {
            public const string Version = "v1";

            public static class Account
            {
                public const string Base = Root + "/" + Version + "/account";

                public const string Login = Base + "/login";
                public const string Logout = Base + "/logout";
                public const string Register = Base + "/register";

            }
        }
    }
}
