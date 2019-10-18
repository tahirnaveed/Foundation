﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Foundation.Campaign.Connected_Services.CouponBlock
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://soap11.api.broadmail")]
    public partial class WebserviceException
    {
        
        private object causeField;
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public object cause
        {
            get
            {
                return this.causeField;
            }
            set
            {
                this.causeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", ConfigurationName="CouponBlock.CouponBlockWebservice")]
    public interface CouponBlockWebservice
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getNameReturn")]
        System.Threading.Tasks.Task<string> getNameAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="removeReturn")]
        System.Threading.Tasks.Task<bool> removeAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="createReturn")]
        System.Threading.Tasks.Task<long> createAsync(string in0, string in1, string in2);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<CouponBlock.getAllIdsResponse> getAllIdsAsync(CouponBlock.getAllIdsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getCodeCountReturn")]
        System.Threading.Tasks.Task<long> getCodeCountAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getUnAssignedCodeCountReturn")]
        System.Threading.Tasks.Task<long> getUnAssignedCodeCountAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getAssignedCodeCountReturn")]
        System.Threading.Tasks.Task<long> getAssignedCodeCountAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getCreatedReturn")]
        System.Threading.Tasks.Task<System.DateTime> getCreatedAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getModifiedReturn")]
        System.Threading.Tasks.Task<System.DateTime> getModifiedAsync(string in0, long in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<CouponBlock.getAssignedMailingsResponse> getAssignedMailingsAsync(CouponBlock.getAssignedMailingsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(CouponBlock.WebserviceException), Action="", Name="fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getVersionReturn")]
        System.Threading.Tasks.Task<string> getVersionAsync();
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllIds", WrapperNamespace="urn:api.broadmail.de/soap11/addons/CouponBlock", IsWrapped=true)]
    public partial class getAllIdsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", Order=0)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", Order=1)]
        public string in1;
        
        public getAllIdsRequest()
        {
        }
        
        public getAllIdsRequest(string in0, string in1)
        {
            this.in0 = in0;
            this.in1 = in1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllIdsResponse", WrapperNamespace="urn:api.broadmail.de/soap11/addons/CouponBlock", IsWrapped=true)]
    public partial class getAllIdsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("getAllIdsReturn")]
        public long[] getAllIdsReturn;
        
        public getAllIdsResponse()
        {
        }
        
        public getAllIdsResponse(long[] getAllIdsReturn)
        {
            this.getAllIdsReturn = getAllIdsReturn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAssignedMailings", WrapperNamespace="urn:api.broadmail.de/soap11/addons/CouponBlock", IsWrapped=true)]
    public partial class getAssignedMailingsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", Order=0)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", Order=1)]
        public long in1;
        
        public getAssignedMailingsRequest()
        {
        }
        
        public getAssignedMailingsRequest(string in0, long in1)
        {
            this.in0 = in0;
            this.in1 = in1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAssignedMailingsResponse", WrapperNamespace="urn:api.broadmail.de/soap11/addons/CouponBlock", IsWrapped=true)]
    public partial class getAssignedMailingsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:api.broadmail.de/soap11/addons/CouponBlock", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("getAssignedMailingsReturn")]
        public long[] getAssignedMailingsReturn;
        
        public getAssignedMailingsResponse()
        {
        }
        
        public getAssignedMailingsResponse(long[] getAssignedMailingsReturn)
        {
            this.getAssignedMailingsReturn = getAssignedMailingsReturn;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface CouponBlockWebserviceChannel : CouponBlock.CouponBlockWebservice, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class CouponBlockWebserviceClient : System.ServiceModel.ClientBase<CouponBlock.CouponBlockWebservice>, CouponBlock.CouponBlockWebservice
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CouponBlockWebserviceClient() : 
                base(CouponBlockWebserviceClient.GetDefaultBinding(), CouponBlockWebserviceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.CouponBlock.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CouponBlockWebserviceClient(EndpointConfiguration endpointConfiguration) : 
                base(CouponBlockWebserviceClient.GetBindingForEndpoint(endpointConfiguration), CouponBlockWebserviceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CouponBlockWebserviceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CouponBlockWebserviceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CouponBlockWebserviceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CouponBlockWebserviceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CouponBlockWebserviceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> getNameAsync(string in0, long in1)
        {
            return base.Channel.getNameAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<bool> removeAsync(string in0, long in1)
        {
            return base.Channel.removeAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<long> createAsync(string in0, string in1, string in2)
        {
            return base.Channel.createAsync(in0, in1, in2);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CouponBlock.getAllIdsResponse> CouponBlock.CouponBlockWebservice.getAllIdsAsync(CouponBlock.getAllIdsRequest request)
        {
            return base.Channel.getAllIdsAsync(request);
        }
        
        public System.Threading.Tasks.Task<CouponBlock.getAllIdsResponse> getAllIdsAsync(string in0, string in1)
        {
            CouponBlock.getAllIdsRequest inValue = new CouponBlock.getAllIdsRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            return ((CouponBlock.CouponBlockWebservice)(this)).getAllIdsAsync(inValue);
        }
        
        public System.Threading.Tasks.Task<long> getCodeCountAsync(string in0, long in1)
        {
            return base.Channel.getCodeCountAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<long> getUnAssignedCodeCountAsync(string in0, long in1)
        {
            return base.Channel.getUnAssignedCodeCountAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<long> getAssignedCodeCountAsync(string in0, long in1)
        {
            return base.Channel.getAssignedCodeCountAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<System.DateTime> getCreatedAsync(string in0, long in1)
        {
            return base.Channel.getCreatedAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<System.DateTime> getModifiedAsync(string in0, long in1)
        {
            return base.Channel.getModifiedAsync(in0, in1);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CouponBlock.getAssignedMailingsResponse> CouponBlock.CouponBlockWebservice.getAssignedMailingsAsync(CouponBlock.getAssignedMailingsRequest request)
        {
            return base.Channel.getAssignedMailingsAsync(request);
        }
        
        public System.Threading.Tasks.Task<CouponBlock.getAssignedMailingsResponse> getAssignedMailingsAsync(string in0, long in1)
        {
            CouponBlock.getAssignedMailingsRequest inValue = new CouponBlock.getAssignedMailingsRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            return ((CouponBlock.CouponBlockWebservice)(this)).getAssignedMailingsAsync(inValue);
        }
        
        public System.Threading.Tasks.Task<string> getVersionAsync()
        {
            return base.Channel.getVersionAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.CouponBlock))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.CouponBlock))
            {
                return new System.ServiceModel.EndpointAddress("http://api.broadmail.de/soap11/addons/CouponBlock");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return CouponBlockWebserviceClient.GetBindingForEndpoint(EndpointConfiguration.CouponBlock);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return CouponBlockWebserviceClient.GetEndpointAddress(EndpointConfiguration.CouponBlock);
        }
        
        public enum EndpointConfiguration
        {
            
            CouponBlock,
        }
    }
}