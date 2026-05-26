using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Dtos.Acbr;

public class EmitirNfeRequestLocal
{
    [JsonPropertyName("ambiente")] public string Ambiente { get; set; } = "homologacao";
    [JsonPropertyName("referencia")] public string Referencia { get; set; } = string.Empty;
    [JsonPropertyName("infNFe")] public InfNFe InfNFe { get; set; } = new();
}

public class InfNFe
{
    [JsonPropertyName("versao")] public string Versao { get; set; } = "4.00";
    [JsonPropertyName("ide")] public Ide Ide { get; set; } = new();
    [JsonPropertyName("emit")] public Emit Emit { get; set; } = new();
    [JsonPropertyName("dest")] public Dest Dest { get; set; } = new();
    [JsonPropertyName("det")] public List<Det> Det { get; set; } = new();
    [JsonPropertyName("total")] public Total Total { get; set; } = new();
    [JsonPropertyName("transp")] public Transp Transp { get; set; } = new();
    [JsonPropertyName("pag")] public Pag Pag { get; set; } = new();
}

public class Ide
{
    public int cUF { get; set; }
    public string cNF { get; set; } = string.Empty;
    public string natOp { get; set; } = string.Empty;
    public int mod { get; set; } = 55;
    public int serie { get; set; }
    public int nNF { get; set; }
    public DateTimeOffset dhEmi { get; set; }
    public int tpNF { get; set; }
    public int idDest { get; set; }
    public string cMunFG { get; set; } = string.Empty;
    public int tpImp { get; set; }
    public int tpEmis { get; set; }
    public int tpAmb { get; set; }
    public int finNFe { get; set; }
    public int indFinal { get; set; }
    public int indPres { get; set; }
    public int procEmi { get; set; }
    public string verProc { get; set; } = "1.0";
}

public class Emit
{
    public string CNPJ { get; set; } = string.Empty;
    public string xNome { get; set; } = string.Empty;
    public string xFant { get; set; } = string.Empty;
    public Endereco enderEmit { get; set; } = new();
    public string IE { get; set; } = string.Empty;
    public int CRT { get; set; }
}

public class Dest
{
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
    public string xNome { get; set; } = string.Empty;
    public int indIEDest { get; set; }
    public Endereco enderDest { get; set; } = new();
}

public class Endereco
{
    public string xLgr { get; set; } = string.Empty;
    public string nro { get; set; } = string.Empty;
    public string xBairro { get; set; } = string.Empty;
    public string cMun { get; set; } = string.Empty;
    public string xMun { get; set; } = string.Empty;
    public string UF { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public string cPais { get; set; } = "1058";
    public string xPais { get; set; } = "BRASIL";
    public string? fone { get; set; }
}

public class Det
{
    public int nItem { get; set; }
    public Prod prod { get; set; } = new();
    public Imposto imposto { get; set; } = new();
}

public class Prod
{
    public string cProd { get; set; } = string.Empty;
    public string cEAN { get; set; } = "SEM GTIN";
    public string xProd { get; set; } = string.Empty;
    public string NCM { get; set; } = string.Empty;
    public string CFOP { get; set; } = string.Empty;
    public string uCom { get; set; } = string.Empty;
    public decimal qCom { get; set; }
    public decimal vUnCom { get; set; }
    public decimal vProd { get; set; }
    public string cEANTrib { get; set; } = "SEM GTIN";
    public string uTrib { get; set; } = string.Empty;
    public decimal qTrib { get; set; }
    public decimal vUnTrib { get; set; }
    public int indTot { get; set; } = 1;
}

public class Imposto
{
    public ICMS ICMS { get; set; } = new();
    public PIS PIS { get; set; } = new();
    public COFINS COFINS { get; set; } = new();
}

public class ICMS
{
    public ICMSSN102 ICMSSN102 { get; set; } = new();
}

public class ICMSSN102
{
    public int orig { get; set; }
    public string CSOSN { get; set; } = "102";
}

public class PIS
{
    public PISNT PISNT { get; set; } = new();
}

public class PISNT
{
    public string CST { get; set; } = "07";
}

public class COFINS
{
    public COFINSNT COFINSNT { get; set; } = new();
}

public class COFINSNT
{
    public string CST { get; set; } = "07";
}

public class Total
{
    public ICMSTot ICMSTot { get; set; } = new();
}

public class ICMSTot
{
    public decimal vBC { get; set; }
    public decimal vICMS { get; set; }
    public decimal vICMSDeson { get; set; }
    public decimal vFCP { get; set; }
    public decimal vBCST { get; set; }
    public decimal vST { get; set; }
    public decimal vFCPST { get; set; }
    public decimal vFCPSTRet { get; set; }
    public decimal vProd { get; set; }
    public decimal vFrete { get; set; }
    public decimal vSeg { get; set; }
    public decimal vDesc { get; set; }
    public decimal vII { get; set; }
    public decimal vIPI { get; set; }
    public decimal vIPIDevol { get; set; }
    public decimal vPIS { get; set; }
    public decimal vCOFINS { get; set; }
    public decimal vOutro { get; set; }
    public decimal vNF { get; set; }
}

public class Transp
{
    public int modFrete { get; set; } = 9;
}

public class Pag
{
    public List<DetPag> detPag { get; set; } = new();
}

public class DetPag
{
    public int indPag { get; set; }
    public string tPag { get; set; } = string.Empty;
    public decimal vPag { get; set; }
}
