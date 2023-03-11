namespace Checkers
{
    public class Soldier
    {
        private Symbol m_Symbol;

        public Soldier(char i_Symbol)
        {
            
            this.m_Symbol = (Symbol)i_Symbol;
        }

        public char GetSymbol()
        {
            return (char)this.m_Symbol;
        }

        public void Coronation()
        {
            this.m_Symbol = (Symbol)((char)m_Symbol == 'O' ? 'Q' : 'Z');
        }

        public bool IsKing()
        {
            bool isNull = this.m_Symbol == null;

            return !isNull && ((char)this.m_Symbol == 'Q' || (char)this.m_Symbol == 'Z');
        }

        enum Symbol
        {
            X,O,Q,Z
        }
    }
}