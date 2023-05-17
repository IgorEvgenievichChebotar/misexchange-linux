namespace ru.novolabs.ExchangeDTOs
{
    public static class DTOInitializer
    {
        public static Work BuildEmptyWork()
        {
            Work work = new Work
            {
                Norm = new Norm()
            };
            return work;
        }
        public static Result BuildEmptyResult()
        {
            Result result = new Result
            {
                Patient = BuildEmptyPatient()
            };
            return result;
        }
        public static Request BuildEmptyRequest()
        {
            Request request = new Request
            {
                Patient = BuildEmptyPatient()
            };
            return request;
        }
        public static Patient BuildEmptyPatient()
        {
            Patient patient = new Patient
            {
                PatientCard = new PatientCard()
            };
            return patient;
        }
    }
}
