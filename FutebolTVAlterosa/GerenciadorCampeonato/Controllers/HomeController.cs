using Campeonato.Aplicacao;
using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using GerenciadorCampeonato.Controllers.CustomFilters;
using Campeonato.Business.Logics;

namespace Campeonato.UI.WEB.Controllers
{
    public class HomeController : Controller
    {
        TokenBusiness tokenBusiness = new TokenBusiness();

        private readonly ClassificacaoAplicacao appClassificacao;
        private readonly PartidaAplicacao appPartida;
        private readonly AcessoAplicacao appAcesso;
        private readonly CampeonatoAplicacao appCampeonato;
        private readonly FotoVideoAplicacao appFotoVideo;

        public ActionResult EnviarMensagem()
        {
            Mensagem_Web msg = new Mensagem_Web();
            if (!string.IsNullOrEmpty(User.Identity.Name))
                return View("~/Views/Home/EnviarMensagem.cshtml", msg);
            else
                return View("~/Views/AdminHome/Login.cshtml");            
        }

        public HomeController()
        {
            appClassificacao = ClassificacaoAplicacaoConstrutor.ClassificacaoAplicacaoADO();
            appPartida = PartidaAplicacaoConstrutor.PartidaAplicacaoADO();
            appAcesso = AcessoAplicacaoConstrutor.AcessoAplicacaoADO();
            appCampeonato = CampeonatoAplicacaoConstrutor.CampeonatoAplicacaoADO();
            appFotoVideo = FotoVideoAplicacaoConstrutor.FotoVideoADO();
        }

        public ActionResult Index()
        {
            appAcesso.AtualizarNumeroAcesso();
            //Listar Chamado Atual
            //var listaProximaRodada = appPartida.ListarUltimaRodada();

            //Listar próxima rodada
            var listaProximaRodada = appPartida.ListarUltimaRodada();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                listaProximaRodada = appPartida.ListarUltimaRodada();
            }
            else
            {
                listaProximaRodada = appPartida.ListarProximaRodada();
            }


            return View(listaProximaRodada);
        }

        public ActionResult Tabela()
        {
            var tabela = appPartida.ListarTodos();
            return View(tabela);
        }

        public ActionResult Galeria()
        {
            var listaFotosVideos = 0;
            return View(listaFotosVideos);
        }

        public ActionResult Contato()
        {

            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        [HttpPost, JsonExceptionFilter]
        public ActionResult Enviar(Mensagem_Web viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Todos os campos são de preenchimento obrigatório.");
                return View("~/Views/Home/EnviarMensagem.cshtml", viewModel);
            }
            else
            {
                viewModel.TickerText = viewModel.Message;
                EnviarMensagemGoogleCloud(viewModel.Message, viewModel.TickerText, viewModel.ContentTitle);
            }

            TempData["successMessage"] = "Mensagem enviada com sucesso.";
            ModelState.Clear();
            viewModel = new Mensagem_Web();
            return View("~/Views/Home/EnviarMensagem.cshtml", viewModel);
        }
        public String EnviarMensagemGoogleCloud(String message, String tickerText, String contentTitle)
        {
            try
            {                
                string deviceId;
                String response = "";
                //Token[] tokens = db.Tokens.ToArray();
                List<Token_Web> tokens = tokenBusiness.ObterToken();

                foreach (Token_Web str in tokens)
                {
                    if (str.TokenStr != null && !str.TokenStr.Equals(""))
                    {
                        deviceId = str.TokenStr.Trim();
                        string postData =
                        "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                          "\"data\": {\"tickerText\":\"" + tickerText + "\", " +
                                     "\"contentTitle\":\"" + contentTitle + "\", " +
                                     "\"message\": \"" + message + "\"}}";
                        
                        response = SendNotification(deviceId, postData);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return "NOK";
            }
        }

        public string SendNotification(string deviceId, string message)
        {
            try
            {
                string SERVER_API_KEY = "AIzaSyCjer6RVxw5b7ypqEOiX0x9-VvIvm5Xg1I";
                var SENDER_ID = deviceId; //"application number";
                var value = message;
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
                Console.WriteLine(postData);
                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                String sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();
                return sResponseFromServer;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        // POST: /Campeonato/SendToken
        //public String SendToken(string token)
        //{
        //    try
        //    {

        //        Token tokenObj = new Token();
        //        tokenObj.TokenStr = token;
        //        tokenObj.TokenId = Guid.NewGuid();
        //        db.Tokens.Add(tokenObj);
        //        db.SaveChanges();
        //        return "OK";
        //    }
        //    catch (Exception e)
        //    {
        //        // Console.WriteLine("Exception: " + e.Message);
        //        return "";
        //    }
        //}
       
    }
}
