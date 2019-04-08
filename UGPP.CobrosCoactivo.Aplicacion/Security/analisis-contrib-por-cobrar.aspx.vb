Imports System.Collections.Specialized
Public Class analisis_contrib_por_cobrar
    Inherits System.Web.UI.Page
    Protected Parameter As New NameValueCollection
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.daMaepre = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.DataSet11 = New DataSet1
        CType(Me.DataSet11, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'daMaepre
        '
        Me.daMaepre.DeleteCommand = Me.SqlDeleteCommand1
        Me.daMaepre.InsertCommand = Me.SqlInsertCommand1
        Me.daMaepre.SelectCommand = Me.SqlSelectCommand1
        Me.daMaepre.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "maepre", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("refe_cata", "refe_cata"), New System.Data.Common.DataColumnMapping("id_propiet", "id_propiet"), New System.Data.Common.DataColumnMapping("direccion", "direccion"), New System.Data.Common.DataColumnMapping("dir_establ", "dir_establ"), New System.Data.Common.DataColumnMapping("mat_inm", "mat_inm"), New System.Data.Common.DataColumnMapping("telefono", "telefono"), New System.Data.Common.DataColumnMapping("area", "area"), New System.Data.Common.DataColumnMapping("area_con", "area_con"), New System.Data.Common.DataColumnMapping("estrato", "estrato"), New System.Data.Common.DataColumnMapping("des_eco", "des_eco"), New System.Data.Common.DataColumnMapping("tarifa", "tarifa"), New System.Data.Common.DataColumnMapping("avaluo", "avaluo"), New System.Data.Common.DataColumnMapping("ult_ava", "ult_ava"), New System.Data.Common.DataColumnMapping("fecha_ua", "fecha_ua"), New System.Data.Common.DataColumnMapping("clase", "clase"), New System.Data.Common.DataColumnMapping("consec", "consec"), New System.Data.Common.DataColumnMapping("tipo", "tipo"), New System.Data.Common.DataColumnMapping("deudanum", "deudanum"), New System.Data.Common.DataColumnMapping("facturable", "facturable"), New System.Data.Common.DataColumnMapping("noproceso", "noproceso")})})
        Me.daMaepre.UpdateCommand = Me.SqlUpdateCommand1
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = "DELETE FROM maepre WHERE (refe_cata = @Original_refe_cata) AND (area = @Original_" & _
        "area OR @Original_area IS NULL AND area IS NULL) AND (area_con = @Original_area_" & _
        "con OR @Original_area_con IS NULL AND area_con IS NULL) AND (avaluo = @Original_" & _
        "avaluo OR @Original_avaluo IS NULL AND avaluo IS NULL) AND (clase = @Original_cl" & _
        "ase OR @Original_clase IS NULL AND clase IS NULL) AND (consec = @Original_consec" & _
        ") AND (des_eco = @Original_des_eco OR @Original_des_eco IS NULL AND des_eco IS N" & _
        "ULL) AND (deudanum = @Original_deudanum OR @Original_deudanum IS NULL AND deudan" & _
        "um IS NULL) AND (dir_establ = @Original_dir_establ OR @Original_dir_establ IS NU" & _
        "LL AND dir_establ IS NULL) AND (direccion = @Original_direccion OR @Original_dir" & _
        "eccion IS NULL AND direccion IS NULL) AND (estrato = @Original_estrato OR @Origi" & _
        "nal_estrato IS NULL AND estrato IS NULL) AND (facturable = @Original_facturable " & _
        "OR @Original_facturable IS NULL AND facturable IS NULL) AND (fecha_ua = @Origina" & _
        "l_fecha_ua OR @Original_fecha_ua IS NULL AND fecha_ua IS NULL) AND (id_propiet =" & _
        " @Original_id_propiet) AND (mat_inm = @Original_mat_inm OR @Original_mat_inm IS " & _
        "NULL AND mat_inm IS NULL) AND (noproceso = @Original_noproceso OR @Original_nopr" & _
        "oceso IS NULL AND noproceso IS NULL) AND (tarifa = @Original_tarifa OR @Original" & _
        "_tarifa IS NULL AND tarifa IS NULL) AND (telefono = @Original_telefono OR @Origi" & _
        "nal_telefono IS NULL AND telefono IS NULL) AND (tipo = @Original_tipo OR @Origin" & _
        "al_tipo IS NULL AND tipo IS NULL) AND (ult_ava = @Original_ult_ava OR @Original_" & _
        "ult_ava IS NULL AND ult_ava IS NULL)"
        Me.SqlDeleteCommand1.Connection = Me.SqlConnection1
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_refe_cata", System.Data.SqlDbType.VarChar, 15, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "refe_cata", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_area", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_area_con", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area_con", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_avaluo", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "avaluo", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_clase", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "clase", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_consec", System.Data.SqlDbType.VarChar, 6, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "consec", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_des_eco", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "des_eco", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_deudanum", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "deudanum", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_dir_establ", System.Data.SqlDbType.VarChar, 34, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "dir_establ", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_direccion", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "direccion", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_estrato", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "estrato", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_facturable", System.Data.SqlDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "facturable", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_fecha_ua", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "fecha_ua", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_id_propiet", System.Data.SqlDbType.VarChar, 15, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "id_propiet", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_mat_inm", System.Data.SqlDbType.VarChar, 18, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "mat_inm", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_noproceso", System.Data.SqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "noproceso", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_tarifa", System.Data.SqlDbType.Decimal, 5, System.Data.ParameterDirection.Input, False, CType(6, Byte), CType(0, Byte), "tarifa", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_telefono", System.Data.SqlDbType.VarChar, 12, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "telefono", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_tipo", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "tipo", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ult_ava", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "ult_ava", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = "INSERT INTO maepre(refe_cata, id_propiet, direccion, dir_establ, mat_inm, telefon" & _
        "o, area, area_con, estrato, des_eco, tarifa, avaluo, ult_ava, fecha_ua, clase, c" & _
        "onsec, tipo, deudanum, facturable, noproceso) VALUES (@refe_cata, @id_propiet, @" & _
        "direccion, @dir_establ, @mat_inm, @telefono, @area, @area_con, @estrato, @des_ec" & _
        "o, @tarifa, @avaluo, @ult_ava, @fecha_ua, @clase, @consec, @tipo, @deudanum, @fa" & _
        "cturable, @noproceso); SELECT refe_cata, id_propiet, direccion, dir_establ, mat_" & _
        "inm, telefono, area, area_con, estrato, des_eco, tarifa, avaluo, ult_ava, fecha_" & _
        "ua, clase, consec, tipo, deudanum, facturable, noproceso FROM maepre WHERE (refe" & _
        "_cata = @refe_cata)"
        Me.SqlInsertCommand1.Connection = Me.SqlConnection1
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@refe_cata", System.Data.SqlDbType.VarChar, 15, "refe_cata"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@id_propiet", System.Data.SqlDbType.VarChar, 15, "id_propiet"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@direccion", System.Data.SqlDbType.VarChar, 50, "direccion"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@dir_establ", System.Data.SqlDbType.VarChar, 34, "dir_establ"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@mat_inm", System.Data.SqlDbType.VarChar, 18, "mat_inm"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@telefono", System.Data.SqlDbType.VarChar, 12, "telefono"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@area", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@area_con", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area_con", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@estrato", System.Data.SqlDbType.VarChar, 2, "estrato"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@des_eco", System.Data.SqlDbType.VarChar, 2, "des_eco"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@tarifa", System.Data.SqlDbType.Decimal, 5, System.Data.ParameterDirection.Input, False, CType(6, Byte), CType(0, Byte), "tarifa", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@avaluo", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "avaluo", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ult_ava", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "ult_ava", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@fecha_ua", System.Data.SqlDbType.DateTime, 8, "fecha_ua"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@clase", System.Data.SqlDbType.VarChar, 2, "clase"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@consec", System.Data.SqlDbType.VarChar, 6, "consec"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@tipo", System.Data.SqlDbType.VarChar, 2, "tipo"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@deudanum", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "deudanum", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@facturable", System.Data.SqlDbType.VarChar, 1, "facturable"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@noproceso", System.Data.SqlDbType.VarChar, 10, "noproceso"))
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = "SELECT refe_cata, id_propiet, direccion, dir_establ, mat_inm, telefono, area, are" & _
        "a_con, estrato, des_eco, tarifa, avaluo, ult_ava, fecha_ua, clase, consec, tipo," & _
        " deudanum, facturable, noproceso FROM maepre WHERE (RTRIM(refe_cata) = @refe_cat" & _
        "a)"
        Me.SqlSelectCommand1.Connection = Me.SqlConnection1
        Me.SqlSelectCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@refe_cata", System.Data.SqlDbType.VarChar))
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = "UPDATE maepre SET refe_cata = @refe_cata, id_propiet = @id_propiet, direccion = @" & _
        "direccion, dir_establ = @dir_establ, mat_inm = @mat_inm, telefono = @telefono, a" & _
        "rea = @area, area_con = @area_con, estrato = @estrato, des_eco = @des_eco, tarif" & _
        "a = @tarifa, avaluo = @avaluo, ult_ava = @ult_ava, fecha_ua = @fecha_ua, clase =" & _
        " @clase, consec = @consec, tipo = @tipo, deudanum = @deudanum, facturable = @fac" & _
        "turable, noproceso = @noproceso WHERE (refe_cata = @Original_refe_cata) AND (are" & _
        "a = @Original_area OR @Original_area IS NULL AND area IS NULL) AND (area_con = @" & _
        "Original_area_con OR @Original_area_con IS NULL AND area_con IS NULL) AND (avalu" & _
        "o = @Original_avaluo OR @Original_avaluo IS NULL AND avaluo IS NULL) AND (clase " & _
        "= @Original_clase OR @Original_clase IS NULL AND clase IS NULL) AND (consec = @O" & _
        "riginal_consec) AND (des_eco = @Original_des_eco OR @Original_des_eco IS NULL AN" & _
        "D des_eco IS NULL) AND (deudanum = @Original_deudanum OR @Original_deudanum IS N" & _
        "ULL AND deudanum IS NULL) AND (dir_establ = @Original_dir_establ OR @Original_di" & _
        "r_establ IS NULL AND dir_establ IS NULL) AND (direccion = @Original_direccion OR" & _
        " @Original_direccion IS NULL AND direccion IS NULL) AND (estrato = @Original_est" & _
        "rato OR @Original_estrato IS NULL AND estrato IS NULL) AND (facturable = @Origin" & _
        "al_facturable OR @Original_facturable IS NULL AND facturable IS NULL) AND (fecha" & _
        "_ua = @Original_fecha_ua OR @Original_fecha_ua IS NULL AND fecha_ua IS NULL) AND" & _
        " (id_propiet = @Original_id_propiet) AND (mat_inm = @Original_mat_inm OR @Origin" & _
        "al_mat_inm IS NULL AND mat_inm IS NULL) AND (noproceso = @Original_noproceso OR " & _
        "@Original_noproceso IS NULL AND noproceso IS NULL) AND (tarifa = @Original_tarif" & _
        "a OR @Original_tarifa IS NULL AND tarifa IS NULL) AND (telefono = @Original_tele" & _
        "fono OR @Original_telefono IS NULL AND telefono IS NULL) AND (tipo = @Original_t" & _
        "ipo OR @Original_tipo IS NULL AND tipo IS NULL) AND (ult_ava = @Original_ult_ava" & _
        " OR @Original_ult_ava IS NULL AND ult_ava IS NULL); SELECT refe_cata, id_propiet" & _
        ", direccion, dir_establ, mat_inm, telefono, area, area_con, estrato, des_eco, ta" & _
        "rifa, avaluo, ult_ava, fecha_ua, clase, consec, tipo, deudanum, facturable, nopr" & _
        "oceso FROM maepre WHERE (refe_cata = @refe_cata)"
        Me.SqlUpdateCommand1.Connection = Me.SqlConnection1
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@refe_cata", System.Data.SqlDbType.VarChar, 15, "refe_cata"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@id_propiet", System.Data.SqlDbType.VarChar, 15, "id_propiet"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@direccion", System.Data.SqlDbType.VarChar, 50, "direccion"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@dir_establ", System.Data.SqlDbType.VarChar, 34, "dir_establ"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@mat_inm", System.Data.SqlDbType.VarChar, 18, "mat_inm"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@telefono", System.Data.SqlDbType.VarChar, 12, "telefono"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@area", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@area_con", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area_con", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@estrato", System.Data.SqlDbType.VarChar, 2, "estrato"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@des_eco", System.Data.SqlDbType.VarChar, 2, "des_eco"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@tarifa", System.Data.SqlDbType.Decimal, 5, System.Data.ParameterDirection.Input, False, CType(6, Byte), CType(0, Byte), "tarifa", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@avaluo", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "avaluo", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ult_ava", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "ult_ava", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@fecha_ua", System.Data.SqlDbType.DateTime, 8, "fecha_ua"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@clase", System.Data.SqlDbType.VarChar, 2, "clase"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@consec", System.Data.SqlDbType.VarChar, 6, "consec"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@tipo", System.Data.SqlDbType.VarChar, 2, "tipo"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@deudanum", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "deudanum", System.Data.DataRowVersion.Current, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@facturable", System.Data.SqlDbType.VarChar, 1, "facturable"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@noproceso", System.Data.SqlDbType.VarChar, 10, "noproceso"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_refe_cata", System.Data.SqlDbType.VarChar, 15, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "refe_cata", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_area", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_area_con", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(14, Byte), CType(3, Byte), "area_con", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_avaluo", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "avaluo", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_clase", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "clase", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_consec", System.Data.SqlDbType.VarChar, 6, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "consec", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_des_eco", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "des_eco", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_deudanum", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "deudanum", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_dir_establ", System.Data.SqlDbType.VarChar, 34, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "dir_establ", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_direccion", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "direccion", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_estrato", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "estrato", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_facturable", System.Data.SqlDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "facturable", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_fecha_ua", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "fecha_ua", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_id_propiet", System.Data.SqlDbType.VarChar, 15, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "id_propiet", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_mat_inm", System.Data.SqlDbType.VarChar, 18, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "mat_inm", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_noproceso", System.Data.SqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "noproceso", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_tarifa", System.Data.SqlDbType.Decimal, 5, System.Data.ParameterDirection.Input, False, CType(6, Byte), CType(0, Byte), "tarifa", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_telefono", System.Data.SqlDbType.VarChar, 12, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "telefono", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_tipo", System.Data.SqlDbType.VarChar, 2, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "tipo", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ult_ava", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(2, Byte), "ult_ava", System.Data.DataRowVersion.Original, Nothing))
        '
        'DataSet11
        '
        Me.DataSet11.DataSetName = "DataSet1"
        Me.DataSet11.Locale = New System.Globalization.CultureInfo("es-CO")
        CType(Me.DataSet11, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents txtRefeCata As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents Validator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Protected WithEvents daMaepre As System.Data.SqlClient.SqlDataAdapter
    Protected WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Protected WithEvents DataSet11 As DataSet1

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim NomServidor, Usuario, Clave, BaseDatos As String
        NomServidor = ConfigurationManager.AppSettings("ServerName")
        Usuario = ConfigurationManager.AppSettings("BD_User")
        Clave = ConfigurationManager.AppSettings("BD_pass")
        BaseDatos = ConfigurationManager.AppSettings("BD_name")

        Me.SqlConnection1.ConnectionString = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
            ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.txtRefeCata.Value = "" Then
            Me.Validator1.ErrorMessage = "Digite algún dato para poder iniciar la impresión"
            Me.Validator1.IsValid = False
            Exit Sub
        Else
            'Se selecciono alguna opcion              
            Me.daMaepre.SelectCommand.Parameters("@refe_cata").Value = Me.txtRefeCata.Value.Trim()
            If Not Me.daMaepre.Fill(Me.DataSet11, "maepre") Then
                'Mostrar mensaje de no hay registros 
                Me.Validator1.ErrorMessage = "No hay datos que coincidan con esta búsqueda"
                Me.Validator1.IsValid = False
            End If

            Response.Redirect("analisis_cont_cobrar_report.aspx?idmaster=" & Me.txtRefeCata.Value.Trim.ToUpper) 'Mandamiento de pago
        End If
    End Sub
End Class
