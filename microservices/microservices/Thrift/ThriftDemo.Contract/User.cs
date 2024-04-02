/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace ThriftDemo.Contract
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class User : TBase
  {
    private bool _IsVIP;
    private string _Remark;

    public long Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public bool IsVIP
    {
      get
      {
        return _IsVIP;
      }
      set
      {
        __isset.IsVIP = true;
        this._IsVIP = value;
      }
    }

    public string Remark
    {
      get
      {
        return _Remark;
      }
      set
      {
        __isset.Remark = true;
        this._Remark = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool IsVIP;
      public bool Remark;
    }

    public User() {
    }

    public User(long Id, string Name, int Age) : this() {
      this.Id = Id;
      this.Name = Name;
      this.Age = Age;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_Id = false;
        bool isset_Name = false;
        bool isset_Age = false;
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.I64) {
                Id = iprot.ReadI64();
                isset_Id = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Name = iprot.ReadString();
                isset_Name = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I32) {
                Age = iprot.ReadI32();
                isset_Age = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Bool) {
                IsVIP = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                Remark = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
        if (!isset_Id)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Id not set");
        if (!isset_Name)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Name not set");
        if (!isset_Age)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Age not set");
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("User");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "Id";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Id);
        oprot.WriteFieldEnd();
        if (Name == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Name not set");
        field.Name = "Name";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
        field.Name = "Age";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Age);
        oprot.WriteFieldEnd();
        if (__isset.IsVIP) {
          field.Name = "IsVIP";
          field.Type = TType.Bool;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(IsVIP);
          oprot.WriteFieldEnd();
        }
        if (Remark != null && __isset.Remark) {
          field.Name = "Remark";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Remark);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("User(");
      __sb.Append(", Id: ");
      __sb.Append(Id);
      __sb.Append(", Name: ");
      __sb.Append(Name);
      __sb.Append(", Age: ");
      __sb.Append(Age);
      if (__isset.IsVIP) {
        __sb.Append(", IsVIP: ");
        __sb.Append(IsVIP);
      }
      if (Remark != null && __isset.Remark) {
        __sb.Append(", Remark: ");
        __sb.Append(Remark);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
