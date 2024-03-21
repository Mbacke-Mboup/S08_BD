namespace S08_Labo.ViewModels
{
    public class NbSpecialiteViewModel
    {
        public NbSpecialiteViewModel(string sp, int nb) { 
        Specialite = sp;
        Nombre = nb;
        }
        public string Specialite { get; set; }
        public int Nombre { get; set; }
    }
}
