﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.Core.Entities
{
    public class Pessoa
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public List<Servico> Servicos { get; set; }
        
    }
}