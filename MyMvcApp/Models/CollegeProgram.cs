namespace MyMvcApp.Models
{
    public class CollegeProgram{
    public int ProgramId { get; set; }
    public string ProgramName{ get; set; }

    public virtual ICollection<Student> Students{ get; set;}
}
}
