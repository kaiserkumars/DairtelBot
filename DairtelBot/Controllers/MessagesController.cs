using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Services.Description;
using System.Collections;
using System.Collections.Generic;

namespace DairtelBot
{
   
    
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
       
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            string replyMessage=string.Empty;
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var phrase = activity.Text;
                var myEntity= string.Empty;
                var luisResponse = await LuisService.PareUserInput(phrase);
                var choice = luisResponse.intents[0].intent;
               // choicesStack.Push(choice);
                //var copyLuisResponseEntity = string.Empty;
                if (luisResponse.intents.Count() > 0)
                {
                    switch (choice)
                    {
                        case "BuySim":
                            myEntity = luisResponse.entities[0].entity;
                            replyMessage = BuySim();
                            break;
                        case "ShowPlans":
                            myEntity = luisResponse.entities[0].type;
                            replyMessage = SelectPlan();
                            break;
                        case "Number":
                            myEntity = luisResponse.entities[0].entity;
                            replyMessage = await GetPlans(myEntity);
                           // replyMessage = checkReply(replyMessage);
                            break;
                        case "DTHPlans":
                            replyMessage = GetDTHType();
                            break;
                      //  case "return":
                        //   theReturnCase();
                          //  break;
                        //case "home":
                          // replyMessage=theHomeCase();
                            //break;
                        default:
                            replyMessage = "Sorry! I don't understand it.";
                            break;
                    
                    }
                }

                else
                {
                    replyMessage = "Sorry! I don't understand it.";
                }

                Activity reply = activity.CreateReply(replyMessage);
                await connector.Conversations.ReplyToActivityAsync(reply);

                //await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
              HandleSystemMessage(activity);
            }


            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

       /////////////////////////////////////////////////////WORK//////////////////////////////////////////////////////////// 
       // FIND A WAY TO STORE PREVIOUS JSON INTENT RETURN FROM LUIS SO THAT IF USER TYPES A NUMBER ACCIDENTLY BOT CAN AVOID TO SHOW PLANS//

        private async Task<string> GetPlans(string data)
        {
            
             // string rechargeType;
            //rechargeType = TypeSearch(data);
            var returnVal = await PlansService.GetPlans(data);
            return returnVal;        
        }

        private string GetDTHType()
        {
            string rvalue = "Choose\n\n1.Monthly Plans\n\n2.Three Months Plan\n\n3.Annual Plan";
            return rvalue;
        }


        private void callPropt()
        {
           
        }
        private string BuySim()
        {
            string data = "Here is the list of documents required to buy a new airtel sim:" +
                " http://www.airtel.in/personal/mobile/prepaid/know-more/documentation-required";
            return data;
        }

        /*
        private string theHomeCase()
        {
            choicesStack.Clear();
           string replyMessage = "Welcome User!\n\nAsk me for mobile recharge plans, dth recharge plans and other FAQs";
            return replyMessage;
        }
        */
        IDictionary<string, string> recType = new Dictionary<string, string>();
        private string SelectPlan()
        {
            string planData = "Enter the serial number corresponding to the desired plan:@" +
                              "1.TOPUP@2.3G/4G DATA@3.2G DATA@4.SMS@5.ROAMING@6.FULL TALKTIME@7.LOCAL-STD-ISD@";
            planData = planData.Replace("@", "\n\n");
            recType.Add("1", "TOPUP");
            recType.Add("2", "3G/4G DATA");
            recType.Add("3", "2G DATA");
            recType.Add("4", "SMS");
            recType.Add("5", "ROAMING");
            recType.Add("6", "FULL TALKTIME");
            recType.Add("7", "LOCAL-STD-ISD");
            return planData;
        }

        /*
        private string checkReply(string replyM)
        {
            if (choicesStack.Peek().ToString() == "ShowPlans")
                return replyM;
            else if (choicesStack.Peek().ToString() == "DTHPlans")
                return replyM;
            else
                return null;
        }*/
        
        private void DownloadData()
        {

        }

        private string TypeSearch(string data)
        {
            if (recType.ContainsKey(data))
                return recType[data];
            else
                return null;
        }

        /*
        private void theReturnCase()
        {
            choicesStack.Pop();
            string popped = choicesStack.Pop().ToString();
            if (string.Equals(popped, "ShowPlans"))
                SelectPlan();
            else if (string.Equals(popped, "DTHPlans"))
                ;
        }*/

        
        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {   

                IConversationUpdateActivity update = message;
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                if(update.MembersAdded!=null && update.MembersAdded.Any())            //Sending the welcome message 
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            var replyMessage = message.CreateReply();
                            replyMessage.Text = $"Welcome {newMember.Name}!\n\nAsk me for mobile recharge plans, dth recharge plans and other FAQs";
                            connector.Conversations.ReplyToActivityAsync(replyMessage);
                        }
                    }
                }

               
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}