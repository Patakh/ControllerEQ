using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ControllerEQ.Model.Data.Context;

public partial class EqContext
{
    public virtual DbSet<PreRecordSave> PreRecordSave { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PreRecordSave>(entity => { entity.HasNoKey(); });
    }

    public async Task<DTicketPrerecord?> PreRecordSaveAsync(long officeId, DTicketPrerecord request)
    {
        try
        {
            var parameterOfficeId = new NpgsqlParameter();
            parameterOfficeId.ParameterName = "in_s_office_id";
            parameterOfficeId.Value = officeId;
            parameterOfficeId.DbType = DbType.Int64;

            var parameterSourceCode = new NpgsqlParameter();
            parameterSourceCode.ParameterName = "in_s_source_prerecord_id";
            parameterSourceCode.Value = 2;
            parameterSourceCode.DbType = DbType.Int64;

            var parameterEmployeeId = new NpgsqlParameter();
            parameterEmployeeId.ParameterName = "in_s_employee_id";
            parameterEmployeeId.Value = DBNull.Value;
            parameterEmployeeId.DbType = DbType.Int64;

            var parameterCustomerFio = new NpgsqlParameter();
            parameterCustomerFio.ParameterName = "in_customer_full_name";
            parameterCustomerFio.Value = request.CustomerFullName;
            parameterCustomerFio.DbType = DbType.String;

            var parameterCustomerPhoneNumber = new NpgsqlParameter();
            parameterCustomerPhoneNumber.ParameterName = "in_customer_phone_number";
            parameterCustomerPhoneNumber.Value = request.CustomerPhoneNumber;
            parameterCustomerPhoneNumber.DbType = DbType.String;

            var parameterCustomerEmail = new NpgsqlParameter();
            parameterCustomerEmail.ParameterName = "in_customer_e_mail";
            parameterCustomerEmail.Value = DBNull.Value;

            var parametrCustomerSnils = new NpgsqlParameter();
            parametrCustomerSnils.ParameterName = "in_customer_snils";
            parametrCustomerSnils.Value = DBNull.Value;

            var parameterStartTime = new NpgsqlParameter();
            parameterStartTime.ParameterName = "in_start_time_prerecord";
            parameterStartTime.Value = request.StartTimePrerecord;

            var parameterStopTime = new NpgsqlParameter();
            parameterStopTime.ParameterName = "in_stop_time_prerecord";
            parameterStopTime.Value = request.StopTimePrerecord;

            var parameterDate = new NpgsqlParameter();
            parameterDate.ParameterName = "in_date_prerecord";
            parameterDate.Value = request.DatePrerecord;
            parameterDate.DbType = DbType.Date;

            var response = await PreRecordSave.FromSqlRaw("SELECT * FROM public.insert_prerecord(@in_s_office_id, @in_s_source_prerecord_id, @in_s_employee_id, " +
                "@in_customer_full_name, @in_customer_phone_number, @in_customer_e_mail, @in_customer_snils," +
                "@in_date_prerecord, @in_start_time_prerecord, @in_stop_time_prerecord)",
                parameterOfficeId, parameterSourceCode, parameterEmployeeId,
                parameterCustomerFio, parameterCustomerPhoneNumber, parameterCustomerEmail, parametrCustomerSnils,
                parameterStartTime, parameterStopTime, parameterDate).FirstAsync();

            return new DTicketPrerecord
            {
                DatePrerecord = request.DatePrerecord,
                StartTimePrerecord = request.StartTimePrerecord,
                StopTimePrerecord = request.StopTimePrerecord,
                CodePrerecord = response.Number
            };
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
