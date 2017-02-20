using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokens_Library;

namespace Exceptions_Library
{


    public enum Exception_Rang
    {
        NaN=-1,
        NonERR=0,
        SyntaxysErr=1,
        SemanticErr=2,
        RuntimeERR=3,
    }

    public enum ErrorExceptionCodes
    {

    }

    public enum WarningExceptionCodes
    {

    }

    public enum MessageExceptionCodes
    {

    }

    public enum CriticalErr_ExceptionCodes
    {

    }


    public abstract class Any_Exception
    {
        protected int StringNumber_InUserCode;
        protected Exception_Rang Exception_Module;
        protected string ExceptionMessage;
        protected Tuple<int, int> NumericRange_InList_Exception;
        protected LinkedList<Token> TokensInException;
        protected string ExceptionTokenText;

        public Any_Exception(IEnumerable<Token> ExceptionTokens, int RowNumber, Exception_Rang ModuleException)
        {
            TokensInException = new LinkedList<Token>(ExceptionTokens);
            NumericRange_InList_Exception = new Tuple<int, int>(TokensInException.First.Value.Range.Item1, TokensInException.Last.Value.Range.Item2);
            StringNumber_InUserCode = TokensInException.First.Value.Row;
            Exception_Module = ModuleException;
            foreach (Token Tok in TokensInException)
                ExceptionTokenText += Tok.Data;
        }

        public Token Get_BaseInfo_AboutTokenID(int TokenIDInList)
        {
            return TokensInException.ElementAt(TokenIDInList);
        }

        public abstract Any_Exception Get_Info_AboutException();

    }

    //public class ErrorException:Any_Exception
    //{
    //    public ErrorExceptionCodes ExceptionCode;
    //}

    //public class WarningException:Any_Exception
    //{
    //    public WarningExceptionCodes ExceptionCode;
    //}

    //public class MessageException:Any_Exception
    //{
    //    public MessageExceptionCodes ExceptionCode;
    //}

    //public class CriticalException:Any_Exception
    //{
    //    public CriticalErr_ExceptionCodes ExceptionCode;
    //}
}
