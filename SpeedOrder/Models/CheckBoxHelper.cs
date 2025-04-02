using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpeedOrder.Models
{
    public static class CheckBoxHelper
    {
        public static async Task HandleCheckBoxChangedAsync(CheckBox checkBox, CheckedChangedEventArgs e, SQLiteAsyncConnection db)
        {
            if (checkBox != null)
            {
                var platillo = checkBox.BindingContext as Platillo;
                if (platillo != null)
                {
                    // Muestra una alerta de confirmación
                    bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                        "Confirmar cambio",
                        "¿Estás seguro de que deseas actualizar la disponibilidad de este platillo?",
                        "Sí", // Opción para confirmar
                        "No"  // Opción para cancelar
                    );

                    // Si el usuario confirma, actualiza el estado
                    if (isConfirmed)
                    {
                        platillo.Disponible = e.Value; // Actualiza el estado de disponibilidad
                        await db.UpdateAsync(platillo); // Guarda los cambios en la base de datos
                    }
                    else
                    {
                        // Si el usuario cancela, restablece el estado del CheckBox
                        checkBox.IsChecked = !e.Value;
                    }
                }
            }
        }
    }
}