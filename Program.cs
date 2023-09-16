﻿using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CursoEFCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(3);
            //db.Clientes.Remove(cliente);
            db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;
            db.SaveChanges();


        }
        private static void AtualizarPedidos()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Cliente Alterado 2";
            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db.Pedidos
                .Include(p=>p.Itens)
                .ThenInclude(p=>p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }
        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem 
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();

        }
        private static void ConsultaDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id> 0 select c).ToList();
            var consultaPorMetodo = db.Clientes.Where(c => c.Id>0).ToList();
            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando cliente: {cliente.Id}");
                db.Clientes.Find(cliente.Id);
            }
        }
        public static void InserirDadosEmMassa() 
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "João Paulo",
                CEP = "49680000",
                Cidade = "Glória",
                Estado = "SE",
                Telefone = "999985656"
            };

            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total de Registro(s): {registros}");
        }
        public static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto); 
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de Registro(s): {registros}");
        }
    }
}
