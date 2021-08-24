using SQLite;
using System;

namespace AppFit.Models
{
    public class Atividade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public double? Peso { get; set; }
        public string Observacoes { get; set; }
    }
}
