using Npgsql;
using NpgsqlTypes;
using System; 
using TVQE.Model; 
using System.Collections.ObjectModel; 

namespace Function;

public static class Tickets 
{ 
    public static ObservableCollection<Ticket> SelectTicketServed(long officeScoreboardId)
    {
        ObservableCollection<Ticket> ticketList = new();
        try
        {
            // Создаем подключение к базе данных
            using (var connection = new NpgsqlConnection(Settings.ConnectionString))
            {
                connection.Open();
                // Создаем команду для вызова функции и получения данных
                using (var command = new NpgsqlCommand("SELECT * FROM public.select_ticket_served(@in_s_office_scoreboard_id)", connection))
                {
                    command.Parameters.AddWithValue("in_s_office_scoreboard_id", NpgsqlDbType.Bigint, officeScoreboardId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ticket ticket = new Ticket();
                            ticket.Id = (long)(reader["out_d_ticket_id"] as long?);
                            ticket.TicketNumberFull = reader["out_ticket_number_full"] as string;
                            ticket.WindowName = reader["out_window_name"] as string;
                            ticketList.Add(ticket);
                        }
                    }
                }
            }
            return ticketList;
        }
        catch (Exception ex)
        {
            return ticketList;
        }
    }
}