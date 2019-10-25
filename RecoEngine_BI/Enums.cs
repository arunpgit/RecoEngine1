using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecoEngine_BI
{
    public class Enums
    {
        public enum UserTypes : int
        {
            administrators = 0,
            operators = 1,
            managers = 2,
            employees = 3,
            customers = 4

        }
        public enum ExpressionType : int
        {
            Filter = 1,
            Opp_ptnl = 2,
            AddColumn = 3,
            Compaign = 4,
            CalaculatedColumn=5,

        }
        public enum Offer_Priority : int
        {
            Take_Up = 1,
            Avg_Ptnl = 2
        }
        public enum Rank_Criteria : int
        {
            Potential = 1,
            Custom = 2
        }
        public enum subscriber_type : int
        {
            Prepaid = 0,
            Postpaid = 1,
        }

        public enum status : int
        {
            Active = 1,
            dormant = 2,
            churned = 3,
            blocked = 4
        }

        public enum DBType : int
        {
            Oracle = 1,
            SQl = 2
        }
        public enum ColType : int
        {
            Key = 1,
            Input = 2,
            Time = 3,
            Segment = 4,
            None = 5
        }


        public enum OpportunityType : int
        {
            RECOMMEND = 0,
            REPLICATE = 1,
            REACTIVATE = 2,
            STIMULATION = 3,
            RETAIN = 4,
            SATISFY = 5,

        }
    }
}
