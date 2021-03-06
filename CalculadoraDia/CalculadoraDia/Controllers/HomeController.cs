using CalculadoraDia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraDia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Invocação da View, em modo GET
        /// </summary>
        /// <returns></returns>
        [HttpGet] // facultativo
        public IActionResult Index()
        {
            // prepara os valores iniciais do visor a 0
            ViewBag.Visor = "0";
            ViewBag.PrimeiroOperador="Sim";
            ViewBag.Operador = "";
            ViewBag.PrimeiroOperando = "";
            ViewBag.LimpaVisor = "Sim";
            return View();

            
        }

        /// <summary>
        /// Invocação da View, em modo POST
        /// </summary>
        /// <param name="botao">operador selecionado pelo utilizador</param>
        /// <param name="visor">valor do Visor dacalculadora</param>
        /// <param name="primeiroOperador">variável auxiliar: já foi escolhido um operador, ou não</param>
        /// <param name="primeiroOperando">variável auxiliar: Primeiro operando a ser utilizado</param>
        /// <param name="operador">variável auxiliar: Operador a ser utilizado na operação</param>
        /// <param name="limpaVisor">variável auxiliar: se "Sim" limpa Visor</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(
            string botao,
            string visor,
            string primeiroOperador,
            string primeiroOperando,
            string operador,
            string limpaVisor)
        {
            
            // avaliar o valor associado à variável "botao"
            switch (botao)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    // atribuir ao Visor o valor do Botão
                    if (visor == "0" || limpaVisor=="Sim")
                        visor = botao;
                    else
                        //visor = visor * 10 + botao;
                        visor = visor + botao; // visor += botao;
                    // marcar que já não preciso de limpar o vsior
                    limpaVisor = "Nao";
                    break;
                case "⁺∕₋":
                    // faz a inversão do valor no inversor
                    if (visor.StartsWith('-'))
                        visor = visor.Substring(1);
                    else
                        if(visor!="0")
                            visor = "-" + visor;
                    break;
                case ",":
                    // faz a gestão da parte decimal do número no visor
                    if (!visor.Contains(','/*botao*/))
                        visor += ",";
                    break;
                case "+":
                case "-":
                case "x":
                case "÷":
                case "=":
                    limpaVisor="Sim"; // marcar o visor como sendo necessário o seu reinício
                    if (primeiroOperador != "Sim")
                    {
                        // esta é,pelo menos , a segunda vez que se se selecionou um operador
                        // efectuar a operação com o operador antigo, e os valores dos operandos
                        double operando1 = Convert.ToDouble(primeiroOperando);
                        double operando2 = Convert.ToDouble(visor); 
                        // efectuar a operação aritmética
                        switch (operador)
                        {
                            case "+":
                                visor = operando1 + operando2 + "";
                                break;
                            case "-":
                                visor = operando1 - operando2 + "";
                                break;
                            case "x":
                                visor = operando1 * operando2 + "";
                                break;
                            case "÷":
                                visor = operando1 / operando2 + "";
                                break;
                        }
                    } // fim do if
                    // armazenar os valores actuais para cáculos futuros
                    // visor actual, que passa a '1º operando'
                    primeiroOperando = visor;
                    // guardar o valor do operador para efectuar a operação
                    operador = botao;
                    // assinalar o que se vai fazer com os operadores
                    if (botao!="=")
                        primeiroOperador = "Nao";
                    else
                        primeiroOperador = "Sim";
                    break;
                case "C":
                    // reset total da calculadora
                    visor = "0";
                    primeiroOperador = "Sim";
                    operador = "";
                    primeiroOperando = "";
                    limpaVisor = "Sim";
                    break;
            } // fim do switch
            // enviar o valor do "visor" para a view
            ViewBag.Visor = visor;
            // preciso manter o estado das variáveis auxiliares
            ViewBag.PrimeiroOperador = primeiroOperador;
            ViewBag.Operador = operador;
            ViewBag.PrimeiroOperando = primeiroOperando;
            ViewBag.LimpaVisor = limpaVisor;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
