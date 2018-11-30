using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Beneficio
{
    public class EventoFolhaBLL : ABLL<EventoFolha, EventoFolhaBLL>
    {
        public override void Validar(EventoFolha pEventoFolha, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pEventoFolha.Descricao.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Evento Folha", "Descrição", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
