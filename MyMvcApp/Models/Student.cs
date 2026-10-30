namespace MyMvcApp.Models
{
    public class Student
    {
    public string StudentNumber { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public int ProgramId{get;set;}

    public virtual CollegeProgram Program { get; set; }

}
}
