using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace bOS.Commons
{
    public class QueryStringHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(QueryStringHelper));


        //Cache
        public static String ID_Token = "TokenId";

        //
        public static String PBX_ATTRIBUTE = "PBX";
        public static String CALLID_ATTRIBUTE = "CALLID";
        public static String CALLERID_ATTRIBUTE = "CALLERID";
        public static String PBX_USER_ATTRIBUTE = "PBX_USER";


        //inserire qui l'elenco di tutti gli attributi registrati su sessione
        public static String ID_ATTRIBUTE = "Id";
        public static String ID_LIST_ATTRIBUTE = "Ids";
        public static String PID_LIST_ATTRIBUTE = "Pids";

        public static String USER_ATTRIBUTE = "User";
        public static String ROLE_ATTRIBUTE = "Role";
        public static String CRUDTYPE_ATTRIBUTE = "Crud";

        public enum ActionType
        {
            NoAction,
            Add,
            Update,
            Delete,
            View,
            Clone
        };

        public static String ACTION_ATTRIBUTE = "Action";
        public static String ADD_ACTION_ATTRIBUTE = "Add";
        public static String UPDATE_ACTION_ATTRIBUTE = "Update";
        public static String DELETE_ACTION_ATTRIBUTE = "Delete";
        public static String VIEW_ACTION_ATTRIBUTE = "View";
        public static String CLONE_ACTION_ATTRIBUTE = "Clone";

        public static Boolean IsPBX(HttpRequest request)
        {
            String PBX = request.QueryString[PBX_ATTRIBUTE];

            if (String.IsNullOrEmpty(PBX))
                return false;

            return true;
        }

        //0 is null
        public static Int32 GetId(HttpRequest request)
        {
            String sId = request.QueryString[ID_ATTRIBUTE];
            if (sId == null) return 0;
            Int32 id = Convert.ToInt32(sId);
            return id;
        }

        public static int GetUser(HttpRequest request)
        {
            String utente = request.QueryString[USER_ATTRIBUTE];
            if (String.IsNullOrEmpty(utente))
                return 0;
            return int.Parse(utente);
        }

        public static string GetUserAsString(HttpRequest request)
        {
            return request.QueryString[USER_ATTRIBUTE] as string;
        }

        public static int GetPaziente(HttpRequest request)
        {
            String utente = request.QueryString["Paziente"];
            if (String.IsNullOrEmpty(utente))
                return 0;
            return int.Parse(utente);
        }

        public static String GetRole(HttpRequest request)
        {
            String utente = request.QueryString[ROLE_ATTRIBUTE];
            return utente;
        }

        public static String GetAction(HttpRequest request)
        {
            String sAction = request.QueryString[ACTION_ATTRIBUTE];
            return sAction;
        }

        public static ActionType GetActionType(HttpRequest request)
        {
            String sAction = request.QueryString[ACTION_ATTRIBUTE];
            if (sAction == null) return ActionType.NoAction;

            if (sAction.Equals(ADD_ACTION_ATTRIBUTE))
                return ActionType.Add;

            else if (sAction.Equals(UPDATE_ACTION_ATTRIBUTE))
                return ActionType.Update;

            else if (sAction.Equals(DELETE_ACTION_ATTRIBUTE))
                return ActionType.Delete;

            else if (sAction.Equals(VIEW_ACTION_ATTRIBUTE))
                return ActionType.View;

            else if (sAction.Equals(CLONE_ACTION_ATTRIBUTE))
                return ActionType.Clone;

            return ActionType.NoAction;

        }

        public static Boolean IsAddAction(HttpRequest request)
        {
            String sAction = request.QueryString[ACTION_ATTRIBUTE];
            if (sAction == null) return false;
            if (sAction.Equals(ADD_ACTION_ATTRIBUTE))
                return true;

            return false;
        }



        public static Int32[] GetIds(HttpRequest request)
        {
            String sId = request.QueryString[ID_LIST_ATTRIBUTE];
            if (sId == null || sId == String.Empty) return null;

            String[] sIds = sId.Split(';');
            List<Int32> vId = new List<Int32>();
            foreach (String item in sIds)
            {
                vId.Add(Int32.Parse(item));
            }
            return vId.ToArray();
        }

        public static String[] GetPids(HttpRequest request)
        {
            String sId = request.QueryString[PID_LIST_ATTRIBUTE];
            if (sId == null || sId == String.Empty) return null;

            return sId.Split(';');
        }

        public static String GetPidsParam(List<String> ids)
        {
            String sIds = String.Join(";", ids.ToArray());

            return String.Format("{0}={1}", PID_LIST_ATTRIBUTE, sIds);

        }
    }
}