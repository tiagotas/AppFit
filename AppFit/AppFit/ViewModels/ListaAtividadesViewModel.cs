using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using AppFit.Models;
using Xamarin.Forms;

namespace AppFit.ViewModels
{
    class ListaAtividadesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Pegando o que foi digitado pelo usuário.
         */
        public string ParametroBusca { get; set; }

        /**
         * Gerencia se mostra ao usuário o RefreshView
         */
        bool estaAtualizando = false;
        public bool EstaAtualizando
        {
            get => estaAtualizando;
            set
            {
                estaAtualizando = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EstaAtualizando"));
            }
        }

        /**
         * Coleção que armazena as atividades do usuário.
         */
        ObservableCollection<Atividade> listaAtividades = new ObservableCollection<Atividade>();
        public ObservableCollection<Atividade> ListaAtividades
        {
            get => listaAtividades;
            set => listaAtividades = value;
        }

        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;

                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.GetAllRows();

                        ListaAtividades.Clear();

                        tmp.ForEach(i => ListaAtividades.Add(i));

                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");

                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }

        public ICommand Buscar
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;

                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.Search(ParametroBusca);

                        ListaAtividades.Clear();

                        tmp.ForEach(i => ListaAtividades.Add(i));

                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }

        public ICommand AbrirDetalhesAtividade
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    await Shell.Current.GoToAsync($"//CadastroAtividade?parametro_id={id}");
                });
            }
        }


        public ICommand Remover
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    try
                    {
                        bool conf = await Application.Current.MainPage.DisplayAlert("Tem Certeza?", "Excluir", "Sim", "Não");

                        if(conf)
                        {
                            await App.Database.Delete(id);
                            AtualizarLista.Execute(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
    }
}
