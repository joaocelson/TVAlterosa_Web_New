using Campeonato.Aplicacao;
using Campeonato.Dominio;
using Campeonato.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Campeonato.UI.WEB.Security;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Campeonato.UI.WEB.Areas.Admin
{
    public class CampeonatoController : Controller
    {
        //
        // GET: /Admin/Campeonato/

        private readonly CampeonatoAplicacao appCampeonato;
        private readonly TimeAplicacao appTimes;
        private readonly NoticiaAplicacao appNoticia;

        private readonly ClassificacaoAplicacao appClassificacao;

        public CampeonatoController()
        {
            appCampeonato = CampeonatoAplicacaoConstrutor.CampeonatoAplicacaoADO();
            appClassificacao = ClassificacaoAplicacaoConstrutor.ClassificacaoAplicacaoADO();
            appNoticia = NoticiaAplicacaoConstrutor.NoticiaAplicacaoADO();
        }

        public ActionResult Index()
        {
            var listaDeCampeonatos = appCampeonato.ListarTodos();
            return View(listaDeCampeonatos);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Cadastrar(Campeonatos campeonato)
        {
            if (ModelState.IsValid)
            {
                appCampeonato.Salvar(campeonato);
                return RedirectToAction("Index");
            }
            return View(campeonato);
        }


        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult SelecionarTimes(string id)
        {
            //var listaDeCampeonatos = appCampeonato.ListarTodos();
            TimeCampeonato timesCampeonato = new TimeCampeonato();
            timesCampeonato.Times = appTimes.ListarTodos();
            timesCampeonato.IdCampeonato = id;
            return View(timesCampeonato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult SelecionarTimes(int id)
        {

            var listaTimes = appTimes.ListarTodos();
            var listaTimesAdicionar = new List<Time>();

            for (int i = 0; i < listaTimes.Count(); i++)
            {
                Time time = listaTimes.ToList<Time>()[i];

                for (int j = 0; j < Request.Form.Count; j++)
                {
                    if (Request.Form.Keys[j].Equals(time.Id.ToString()))
                    {
                        var checkbox = Request.Form[j];
                        if (checkbox != "false")
                        {
                            time.SelecionadoCampeonato = true;
                            listaTimesAdicionar.Add(time);
                        }
                    }
                }
            }
            //Arrumar - JOAO CELSON

            appCampeonato.AdicionarTimes(listaTimesAdicionar, id.ToString());
            return RedirectToAction("TimesPorCampeonato");
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Editar(string id)
        {
            var campeonato = appCampeonato.ListarPorId(id);

            if (campeonato == null)
                return HttpNotFound();

            return View(campeonato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Editar(Campeonatos campeonato)
        {
            if (ModelState.IsValid)
            {
                appCampeonato.Salvar(campeonato);
                return RedirectToAction("Index");
            }
            return View(campeonato);
        }

        public ActionResult Detalhes(string id)
        {
            var campeonato = appCampeonato.ListarPorId(id);

            if (campeonato == null)
                return HttpNotFound();

            return View(campeonato);
        }

        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult Excluir(string id)
        {
            var campeonato = appCampeonato.ListarPorId(id);

            if (campeonato == null)
                return HttpNotFound();

            return View(campeonato);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        [PermissoesFiltro(Roles = "Admin")]
        public ActionResult ExcluirConfirmado(string id)
        {
            var campeonato = appCampeonato.ListarPorId(id);
            appCampeonato.Excluir(campeonato);
            return RedirectToAction("Index");
        }

        public ActionResult TimesPorCampeonato(string idCampeonato)
        {
            TimeCampeonato timeCampeonato = new TimeCampeonato();
            timeCampeonato.Times = appTimes.ListarTimesCampeonato(idCampeonato);
            timeCampeonato.IdCampeonato = idCampeonato;
            return View(timeCampeonato);
        }

        public ActionResult Tabela(string id)
        {
            return RedirectToAction("Tabela", new RouteValueDictionary(
    new { controller = "Partida", action = "Tabela", Id = id }));

        }

        public ActionResult Classificacao(string id)
        {
            var listaClassificacao = appClassificacao.ListarClassicacaoPorCampeonato(id);
            return View(listaClassificacao);
        }

        public ActionResult CampeoesPorCampeonato(string id)
        {
            var listaCampeos = appClassificacao.ListarCampeoesPorCampeonato(id);
            return View(listaCampeos);
        }

        public ActionResult EnviarMensagemGoogle()
        {
            return View();
        }


        //JSON - Retorna todos os dados JSON
        //=======================================================

        public String ObterClassificacaoJson(string id)
        {
            List<Classificacao> listaClassificacao = (List<Classificacao>)appClassificacao.ListarClassicacaoPorCampeonato(id);

            //if (id.Equals("3"))
            //{
            //    Classificacao classificacaoA = new Classificacao();
            //    classificacaoA.Posicao = "";
            //    classificacaoA.NomeTime = "GRUPO A";
            //    classificacaoA.Pontos = "";
            //    listaClassificacao.Insert(0, classificacaoA);

            //    List<Classificacao> listaClassificacaoB = (List<Classificacao>)appClassificacao.ListarClassicacaoPorCampeonato("4");
            //    Classificacao classificacao = new Classificacao();
            //    classificacao.Posicao = "";
            //    classificacao.NomeTime = "GRUPO B";
            //    classificacao.Pontos = "";
            //    listaClassificacao.Add(classificacao);
            //    foreach (Classificacao b in listaClassificacaoB)
            //    {
            //        listaClassificacao.Add(b);
            //    }

            //}
            Classificacao classificacao2 = new Classificacao();
            classificacao2.Posicao = " ";
            classificacao2.NomeTime = " ";
            classificacao2.Pontos = " ";
            listaClassificacao.Add(classificacao2);

            return JsonConvert.SerializeObject(listaClassificacao, Formatting.Indented);
        }

        public String ObterArtilhariaJson(string id)
        {
            var listaArtilheiros = appCampeonato.ArtilhariaPorCampeonato(id);
            Artilheiro artilheiro = new Artilheiro();
            artilheiro.Nome = " ";
            artilheiro.NumeroGols = "0";
            artilheiro.Time = " ";
            List<Artilheiro> artilheiros =  listaArtilheiros.ToList<Artilheiro>();
            artilheiros.Add(artilheiro);
            return JsonConvert.SerializeObject(artilheiros, Formatting.Indented);
        }

        public String ObterArtilhariaGeralJson()
        {
            var listaArtilheiros = appCampeonato.ArtilhariaPorCampeonatoGeral();
            Artilheiro artilheiro = new Artilheiro();
            artilheiro.Nome = " ";
            artilheiro.NumeroGols = "0";
            artilheiro.Time = " ";
            List<Artilheiro> artilheiros = listaArtilheiros.ToList<Artilheiro>();
            artilheiros.Add(artilheiro);
            return JsonConvert.SerializeObject(artilheiros, Formatting.Indented);
        }

        public String Noticias()
        {
            var listaNoticias = appNoticia.ListarTodos();
            return JsonConvert.SerializeObject(listaNoticias, Formatting.Indented);
        }

        public String AdicionarNoticia(Noticia noticia)
        {
            try
            {
                appNoticia.Salvar(noticia);
                return "OK";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //GETIMAGENS
        public ActionResult Image(string nomeImagem)
        {
            var dir = Server.MapPath("/Content/images/escudo");
            var path = Path.Combine(dir, nomeImagem);
            return base.File(path, "image/jpg");
        }

        public FileResult Download()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("/apk/apk.apk"));
            string fileName = "A_ibititec.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        //SEND TOKEN 
        //=======================================================================================

        //GOOGLE CLOUD MESSAGE  -- SEND MESSAGE FOR SERVER

        // POST: /Campeonato/Create
        public String EnviarMensagemGoogleCloud(String message, String tickerText, String contentTitle)
        {
            try
            {
                //string deviceId = "f0tnwv5yu5w:APA91bH9NbXK-FeCvDw1gBSnq_sKNRxrv20iPEF1p6aC_zZ4z3LSFF7Fv5KY1UQGiL-f4LO954FQWbooOQL_rJFJ8FcvNlnIy9yItmb4Yp-sX-EAVMb2dBAuk_-JO2Vf73S0V3GxIxOp";
                string deviceId;
                //string message = "Atualizado os resultados da última rodada";
                //string tickerText = "Atualização Classificação Campeonato";
                //string contentTitle = "FUTEBOL LD - Atualização Resultados e Classificação";
                String response = "";
                List<String> tokens = appCampeonato.ObterTokens();

                //string text = System.IO.File.ReadAllText((Server.MapPath("/docs/tokens.txt")));

                foreach (string str in tokens)
                {
                    if (!str.Equals(""))
                    {
                        deviceId = str.Trim();
                        string postData =
                        "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
                          "\"data\": {\"tickerText\":\"" + tickerText + "\", " +
                                     "\"contentTitle\":\"" + contentTitle + "\", " +
                                     "\"message\": \"" + message + "\"}}";

                        //            string response = SendGCMNotification("AIzaSyCo_YCF3pzU6VL8e8quJxmnQZBAMyfvzkk", postData);
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
                //string SERVER_API_KEY = "AIzaSyDdtnM5NR9L7d2HEy63bjcX4FIi2-bPMw0"; 
                string SERVER_API_KEY = "AIzaSyCs-QxqvnCeabDKNzYTE8HK7ZAS6eZaoN4";
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

        // POST: /Campeonato/Create
        public String SendToken(string token)
        {
            try
            {
                bool retorno = appCampeonato.GravarToken(token);

                if (retorno)
                {
                    return "OK";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine("Exception: " + e.Message);
                return "";
            }
        }


    }
}
