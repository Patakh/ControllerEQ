using Microsoft.EntityFrameworkCore;
using Npgsql;
using QE.FunctionContext;
using QE.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QE.Context;

public partial class EqContext
{
    public virtual DbSet<DTicketSaveResponse> DTicketResponse { get; set; }
    public virtual DbSet<PreRecord> PreRecords { get; set; }
    public virtual DbSet<PreRecordSave> PreRecordSave { get; set; }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DTicketSaveResponse>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<PreRecord>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<PreRecordSave>(entity => { entity.HasNoKey(); });
    }

    public async Task<DTicketSaveResponse?> TiketSaveAsync(TiketSaveRequestDto requestModel)
    {
        try
        {
            var parametrOfficeId = new NpgsqlParameter();
            parametrOfficeId.ParameterName = "in_s_office_id";
            parametrOfficeId.Value = requestModel.OfficeId;
            parametrOfficeId.DbType = DbType.Int64;

            var parametrTerminalId = new NpgsqlParameter();
            parametrTerminalId.ParameterName = "in_s_office_terminal_id";
            parametrTerminalId.Value = requestModel.OfficeTerminalId;
            parametrTerminalId.DbType = DbType.Int64;

            var parametrServiceId = new NpgsqlParameter();
            parametrServiceId.ParameterName = "in_s_service_id";
            parametrServiceId.Value = requestModel.ServiceId;
            parametrServiceId.DbType = DbType.Int64;

            var parametrPriorityId = new NpgsqlParameter();
            parametrPriorityId.ParameterName = "in_s_priority_id";
            parametrPriorityId.Value = requestModel.PriorityId.HasValue ? requestModel.PriorityId.Value : DBNull.Value;
            parametrPriorityId.DbType = DbType.Int64;

            var parametrPrerecordId = new NpgsqlParameter();
            parametrPrerecordId.ParameterName = "in_d_ticket_prerecord_id";
            parametrPrerecordId.Value = requestModel.PrerecordId.HasValue ? requestModel.PrerecordId.Value : DBNull.Value;
            parametrPrerecordId.DbType = DbType.Int64;

            return await DTicketResponse.FromSqlRaw("SELECT * FROM public.insert_ticket(@in_s_office_id, @in_s_office_terminal_id, @in_s_service_id, @in_s_priority_id, @in_d_ticket_prerecord_id)", 
                parametrOfficeId, parametrTerminalId, parametrServiceId, parametrPriorityId, parametrPrerecordId).FirstAsync();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<List<PreRecord>?> GetPreRecordData(long officeId)
    {
        try
        {
            var parametrOfficeId = new NpgsqlParameter();
            parametrOfficeId.ParameterName = "in_s_office_id";
            parametrOfficeId.Value = officeId;
            parametrOfficeId.DbType = DbType.Int64;

            var parametrDate = new NpgsqlParameter();
            parametrDate.ParameterName = "in_date";
            parametrDate.Value = DateTime.Now.Date;
            parametrDate.DbType = DbType.Date;

            return await PreRecords.FromSqlRaw("SELECT * FROM public.select_prerecord(@in_s_office_id, @in_date)", parametrOfficeId, parametrDate).ToListAsync();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<PreRecordSaveResponseDto?> PreRecordSaveAsync(long officeId,PreRecordSaveRequestDto request)
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
            parameterCustomerFio.Value = request.Fio;
            parameterCustomerFio.DbType = DbType.String;

            var parameterCustomerPhoneNumber = new NpgsqlParameter();
            parameterCustomerPhoneNumber.ParameterName = "in_customer_phone_number";
            parameterCustomerPhoneNumber.Value = request.PhoneNumber;
            parameterCustomerPhoneNumber.DbType = DbType.String;

            var parameterCustomerEmail = new NpgsqlParameter();
            parameterCustomerEmail.ParameterName = "in_customer_e_mail";
            parameterCustomerEmail.Value = DBNull.Value;

            var parametrCustomerSnils = new NpgsqlParameter();
            parametrCustomerSnils.ParameterName = "in_customer_snils";
            parametrCustomerSnils.Value = DBNull.Value;

            var parameterStartTime = new NpgsqlParameter();
            parameterStartTime.ParameterName = "in_start_time_prerecord";
            parameterStartTime.Value = TimeOnly.FromTimeSpan(request.StartTimePrerecord);

            var parameterStopTime = new NpgsqlParameter();
            parameterStopTime.ParameterName = "in_stop_time_prerecord";
            parameterStopTime.Value = TimeOnly.FromTimeSpan(request.StopTimePrerecord);

            var parameterDate = new NpgsqlParameter();
            parameterDate.ParameterName = "in_date_prerecord";
            parameterDate.Value = request.DatePreRecord.Date;
            parameterDate.DbType = DbType.Date;

            var response = await PreRecordSave.FromSqlRaw("SELECT * FROM public.insert_prerecord(@in_s_office_id, @in_s_source_prerecord_id, @in_s_employee_id, " +
                "@in_customer_full_name, @in_customer_phone_number, @in_customer_e_mail, @in_customer_snils," +
                "@in_date_prerecord, @in_start_time_prerecord, @in_stop_time_prerecord)",
                parameterOfficeId, parameterSourceCode, parameterEmployeeId, 
                parameterCustomerFio, parameterCustomerPhoneNumber, parameterCustomerEmail, parametrCustomerSnils,
                parameterStartTime, parameterStopTime, parameterDate).FirstAsync();

          return new PreRecordSaveResponseDto
            {
                DatePreRecord = request.DatePreRecord,
                StartTimePrerecord = request.StartTimePrerecord,
                StopTimePrerecord = request.StopTimePrerecord,
                Number = response.Number
            };

        }
        catch (Exception e)
        {
            return null;
        }
    }


}
