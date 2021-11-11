using Data.Entities;
using Data.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Service.Dto;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Service.Services
{
    public class ExtratoClienteService : IExtratoClienteService
    {
        IExtratoClienteInterface _IExtratoCliente;

        public bool ProcessaPdf(ExtratoPdfDto dto)
        {
            using (PdfReader reader = new PdfReader(dto.File))
            {
                PdfDocument pdf = new PdfDocument(reader);
                ExtratoCliente extrato = new ExtratoCliente();

                switch (dto.LayoutPdf)
                {
                    case "bradesco":
                        extrato = DecodificaLayoutBradesco_OLD(pdf);
                        break;
                    case "itau":
                        extrato = DecodificaLayoutItau(pdf);
                        break;
                    default:
                        break;
                }

                if(extrato != null)
                    _IExtratoCliente.Add(extrato);
            }

            return true;
        }

        #region Mapeamento Layouts

        #region Bradesco
        [Obsolete]
        private ExtratoCliente DecodificaLayoutBradesco_OLD(PdfDocument pdf)
        {
            ExtratoCliente dtoRetorno = new ExtratoCliente();
            dtoRetorno.ModeloExtrato = "Bradesco";

            //Regex para remover espaços duplos
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            var capa = getContentInList(PdfTextExtractor.GetTextFromPage(pdf.GetFirstPage()));
            dtoRetorno.Agencia = capa[17].Substring(9);
            dtoRetorno.ContaCorrente = capa[19].Substring(16);

            for (int pagNumber = 6; pagNumber <= pdf.GetNumberOfPages(); pagNumber++)
            {
                var page = pdf.GetPage(pagNumber);
                var pageContentList = getContentInList(regex.Replace(PdfTextExtractor.GetTextFromPage(page), " "));

                if (pageContentList[0] == "Posição Analítica de Renda Variável")
                {
                    var tipo = pageContentList[2];
                    //seleciona somente as linhas da tabela
                    var InicioLista = pageContentList.FindIndex(x => x.Contains("Provisionados Líquido")) + 2;
                    var finalLista = pageContentList.FindIndex(x => x.Contains("Total Bolsa")) - 2;

                    for (int linha = InicioLista; linha <= finalLista; linha++)
                    {
                        var textoLinha = pageContentList[linha];
                        textoLinha = textoLinha.Replace(' ', ';');
                        var dados = textoLinha.Split(';');

                        dtoRetorno.PosicaoCliente.Add(new PosicaoCliente { TipoAtivo = tipo, Ativo = dados[0], ValorFinanceiroLiq = decimal.Parse(dados[dados.Count() - 3]) });
                    }
                }
                else if (pageContentList[0] == "Movimentação dos Títulos Renda Variável")
                {
                    var tipoAtivo = pageContentList[2];
                    //seleciona somente as linhas da tabela
                    var InicioLista = pageContentList.FindIndex(x => x.Contains("Movimento")) + 2;
                    var finalLista = pageContentList.FindIndex(x => x.Contains("Resumo")) - 1;

                    for (int linha = InicioLista; linha <= finalLista; linha++)
                    {
                        var textoLinha = pageContentList[linha];
                        textoLinha = textoLinha.Replace(' ', ';');
                        var dados = textoLinha.Split(';');

                        dtoRetorno.MovimentacaoCliente.Add(new MovimentacaoCliente {DataMovimentacao = DateTime.Parse(pageContentList[linha - 1]), TipoMovimentacao = dados[0], TipoAtivo = tipoAtivo, Ativo = dados[1], ValorFinanceiroLiq = decimal.Parse(dados[dados.Count() - 1]) });
                    }
                }
                else if (pageContentList[0] == "Posição Analítica dos Fundos de Investimento")
                {
                    var tipo = pageContentList[2];
                    var index = pageContentList.FindAll(x => x == "Prazo");
                }
            }

            return dtoRetorno;
        }

        private ExtratoCliente DecodificaLayoutBradesco(PdfDocument pdf)
        {
            ExtratoCliente dtoRetorno = new ExtratoCliente();
            dtoRetorno.ModeloExtrato = "Bradesco";

            var linhasPagina = getContentInList(PdfTextExtractor.GetTextFromPage(pdf.GetFirstPage()));
            Regex pattern = new Regex(@"Ag: (?<agencia>\d+) Conta: (?<conta>\d+(\-\d+))");
            Match match = pattern.Match(linhasPagina[2]);

            dtoRetorno.Agencia = match.Groups["agencia"].Value;
            dtoRetorno.ContaCorrente = match.Groups["conta"].Value;

            for (int pagNumber = 6; pagNumber <= pdf.GetNumberOfPages(); pagNumber++)
            {
                var page = pdf.GetPage(pagNumber);
                linhasPagina = getContentInList(PdfTextExtractor.GetTextFromPage(page));

                if (linhasPagina[0] == "V. Posição Detalhada dos Investimentos")
                {
                    //dtoRetorno.PosicaoCliente = PreenchePosicaoClienteItau(linhasPagina, "Renda Variável");
                    //dtoRetorno.PosicaoCliente.Concat(PreenchePosicaoClienteItau(linhasPagina, "Renda Fixa"));
                }
                if (linhasPagina[0] == "IX. Movimentação da Carteira de Investimento")
                {
                    //dtoRetorno.MovimentacaoCliente = PreencheMovimentacoesClienteItau(linhasPagina);
                }
            }

                return dtoRetorno;
        }
        #endregion

        #region Itau
        private ExtratoCliente DecodificaLayoutItau(PdfDocument pdf)
        {
            ExtratoCliente dtoRetorno = new ExtratoCliente();
            dtoRetorno.ModeloExtrato = "Itau";

            var pagina = getContentInList(PdfTextExtractor.GetTextFromPage(pdf.GetPage(2)));
            var dadoscliente = pagina[2].Substring(pagina[2].IndexOf(':') + 2).Split('/');
            dtoRetorno.Agencia = dadoscliente[0];
            dtoRetorno.ContaCorrente = dadoscliente[1];

            for (int pagNumber = 4; pagNumber <= pdf.GetNumberOfPages(); pagNumber++)
            {
                var page = pdf.GetPage(pagNumber);
                pagina = getContentInList(PdfTextExtractor.GetTextFromPage(page));

                if (pagina[1] == "Detalhamento")
                {
                    dtoRetorno.PosicaoCliente = PreenchePosicaoClienteItau(pagina, "Renda Variável");
                    dtoRetorno.PosicaoCliente.Concat(PreenchePosicaoClienteItau(pagina, "Renda Fixa"));
                }
                if (pagina[1] == "Movimentação")
                {
                    dtoRetorno.MovimentacaoCliente = PreencheMovimentacoesClienteItau(pagina);
                }
            }

            return dtoRetorno;
        }

        private List<PosicaoCliente> PreenchePosicaoClienteItau(List<string> pagina, string tipoAtivo)
        {
            List<PosicaoCliente> listPosicaoCliente = new List<PosicaoCliente>();
            PosicaoCliente posicaoCliente = new PosicaoCliente();

            int inicioListaAtivos = 0, finalListaAtivos = 0;
            string classificacaoAtivo = string.Empty;

            switch (tipoAtivo)
            {
                case "Renda Variável":
                    inicioListaAtivos = pagina.FindIndex(x => x.Contains("Renda Variável Qtd. IR (R$) IOF (R$)")) + 3;
                    finalListaAtivos = pagina.FindIndex(x => x.Contains("Total Renda Variável")) - 1;
                    break;
                case "Renda Fixa":
                    inicioListaAtivos = pagina.FindIndex(x => x.Contains("Renda Fixa Qtd. IR (R$) IOF (R$)")) + 3;
                    finalListaAtivos = pagina.FindIndex(x => x.Contains("Total Renda Fixa")) - 1;
                    break;
                default:
                    break;
            }

            for (int linha = inicioListaAtivos; linha <= finalListaAtivos; linha++)
            {
                var textoLinha = pagina[linha];
                bool linhaContemValores = textoLinha.Contains(',');

                textoLinha = textoLinha.Replace(' ', ';');
                var dados = textoLinha.Split(';');

                if ((textoLinha.Contains("-;-;-;-;-;") && tipoAtivo == "Renda Fixa") || (textoLinha.Contains("-;-;-;") && tipoAtivo == "Renda Variável"))
                    classificacaoAtivo = dados[0];
                else if (textoLinha.Contains("....") == false)//Caso seja verdadeiro é o próximo ativo da tabela
                {
                    if(!(dados[0] == "-" && dados[1] == "-"))//Linhas que possuem quebra, não vem parte do nome do ativo na linha com os valores
                    {
                        var ativo = string.Empty;
                        for (int i = 0; i < dados.Length; i++)
                        {
                            if (dados[i].Contains(','))
                                break;
                            else
                                ativo += dados[i] + " ";
                        }

                        posicaoCliente.Ativo += ativo;
                    }

                    if (linhaContemValores)
                    {
                        posicaoCliente.TipoAtivo = tipoAtivo;
                        posicaoCliente.ClassificacaoAtivo = classificacaoAtivo;
                        posicaoCliente.ValorFinanceiroLiq = decimal.Parse(dados[dados.Count() - 7]);
                    }
                }
                
                if (textoLinha.Contains("....") || linha == finalListaAtivos)
                {
                    listPosicaoCliente.Add(posicaoCliente);
                    posicaoCliente = new PosicaoCliente();
                }
            }

            return listPosicaoCliente;
        }

        private List<MovimentacaoCliente> PreencheMovimentacoesClienteItau(List<string> pagina)
        {
            List<MovimentacaoCliente> listMovimentacaoCliente = new List<MovimentacaoCliente>();

            var tipo = pagina[2];
            //seleciona somente as linhas da tabela
            var InicioLista = pagina.FindIndex(x => x.Contains("receitas (R$)")) + 1;
            var finalLista = pagina.FindIndex(x => x.Contains("Aplicações - resgates")) - 1;

            for (int linha = InicioLista; linha <= finalLista; linha += 2)
            {
                var textoLinha = pagina[linha];
                textoLinha = textoLinha.Replace(' ', ';');
                var dados = textoLinha.Split(';');

                var ativo = string.Empty;
                for (int i = 2; i < dados.Length; i++)
                {
                    if (dados[i].Contains(','))
                    {
                        ativo = ativo[ativo.Length - 2] == '-' ? ativo.Substring(0, ativo.Length - 3) : ativo;
                        break;
                    }
                    else
                        ativo += dados[i] + " ";
                }

                listMovimentacaoCliente.Add(new MovimentacaoCliente { DataMovimentacao = DateTime.Parse(dados[0]), TipoMovimentacao = dados[1], TipoAtivo = "", Ativo = ativo, ValorFinanceiroLiq = decimal.Parse(dados[dados.Count() - 2]) });
            }

            return listMovimentacaoCliente;
        }

        #endregion

        #endregion

        private List<string> getContentInList(string pdfContent)
        {
            return pdfContent.Split('\n').ToList();
        }

        #region Contructor
        public ExtratoClienteService(IExtratoClienteInterface IExtratoCliente)
        {
            _IExtratoCliente = IExtratoCliente;
        }
        #endregion

        #region Comom Interface Methods
        public void Add(ExtratoCliente Entity)
        {
            _IExtratoCliente.Add(Entity);
        }

        public List<ExtratoCliente> GetAll()
        {
            return _IExtratoCliente.GetAll();
        }

        public ExtratoCliente GetById(int Id)
        {
            return _IExtratoCliente.GetById(Id);
        }

        public void Remove(ExtratoCliente Entity)
        {
            _IExtratoCliente.Remove(Entity);
        }

        public void Update(ExtratoCliente Entity)
        {
            _IExtratoCliente.Update(Entity);
        }
        #endregion
    }
}
