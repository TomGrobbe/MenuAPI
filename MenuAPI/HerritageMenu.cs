namespace MenuAPI
{
    public enum Dad
    {
        Benjamin, Daniel, Joshua, Noah, Andrew, Juan, Alex, Isaac, Evan, Ethan, Vincent, Angel, Diego, Adrian, Gabriel, Michael, Santiago, Kevin, Louis, Samuel, Anthony, Claude, Niko
    }

    public enum Mum
    {
        Hannah, Aubrey, Jasmine, Gisele, Amelia, Isabella, Zoe, Ava, Camila, Violet, Sophia, Evelyn, Nicole, Ashley, Gracie, Brianna, Natalie, Olivia, Elizabeth, Charlotte, Emma, Misty
    }

    public class HerritageMenu : MenuAPI.Menu
    {
        public Mum CurrentMum;
        public Dad CurrentDad;

        public HerritageMenu(string name) : base(name)
        {
        }

        public HerritageMenu(string name, string subtitle) : base(name, subtitle)
        {
        }
    }
}
