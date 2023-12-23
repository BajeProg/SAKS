﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторного создания кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Order", Namespace="http://schemas.datacontract.org/2004/07/DeliveryServiceLibrary.Models")]
    public partial class Order : object
    {
        
        private string AddressField;
        
        private int HeightField;
        
        private int IdField;
        
        private int LengthField;
        
        private float PriceField;
        
        private string ReceiverField;
        
        private int SenderIdField;
        
        private float WeightField;
        
        private int WidthField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Address
        {
            get
            {
                return this.AddressField;
            }
            set
            {
                this.AddressField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Height
        {
            get
            {
                return this.HeightField;
            }
            set
            {
                this.HeightField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Length
        {
            get
            {
                return this.LengthField;
            }
            set
            {
                this.LengthField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float Price
        {
            get
            {
                return this.PriceField;
            }
            set
            {
                this.PriceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Receiver
        {
            get
            {
                return this.ReceiverField;
            }
            set
            {
                this.ReceiverField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SenderId
        {
            get
            {
                return this.SenderIdField;
            }
            set
            {
                this.SenderIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float Weight
        {
            get
            {
                return this.WeightField;
            }
            set
            {
                this.WeightField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Width
        {
            get
            {
                return this.WidthField;
            }
            set
            {
                this.WidthField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OrderService.IOrderService")]
    public interface IOrderService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/Get", ReplyAction="http://tempuri.org/IOrderService/GetResponse")]
        System.Threading.Tasks.Task<OrderService.Order[]> GetAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/Add", ReplyAction="http://tempuri.org/IOrderService/AddResponse")]
        System.Threading.Tasks.Task<bool> AddAsync(OrderService.Order order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/Remove", ReplyAction="http://tempuri.org/IOrderService/RemoveResponse")]
        System.Threading.Tasks.Task RemoveAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/Edit", ReplyAction="http://tempuri.org/IOrderService/EditResponse")]
        System.Threading.Tasks.Task EditAsync(OrderService.Order order);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface IOrderServiceChannel : OrderService.IOrderService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class OrderServiceClient : System.ServiceModel.ClientBase<OrderService.IOrderService>, OrderService.IOrderService
    {
        
        /// <summary>
        /// Реализуйте этот разделяемый метод для настройки конечной точки службы.
        /// </summary>
        /// <param name="serviceEndpoint">Настраиваемая конечная точка</param>
        /// <param name="clientCredentials">Учетные данные клиента.</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public OrderServiceClient() : 
                base(OrderServiceClient.GetDefaultBinding(), OrderServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IOrderService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OrderServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(OrderServiceClient.GetBindingForEndpoint(endpointConfiguration), OrderServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OrderServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(OrderServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OrderServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(OrderServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OrderServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<OrderService.Order[]> GetAsync()
        {
            return base.Channel.GetAsync();
        }
        
        public System.Threading.Tasks.Task<bool> AddAsync(OrderService.Order order)
        {
            return base.Channel.AddAsync(order);
        }
        
        public System.Threading.Tasks.Task RemoveAsync(int id)
        {
            return base.Channel.RemoveAsync(id);
        }
        
        public System.Threading.Tasks.Task EditAsync(OrderService.Order order)
        {
            return base.Channel.EditAsync(order);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IOrderService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Не удалось найти конечную точку с именем \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IOrderService))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:8080/OrderService/DeliveryServiceLibrary.OrderService");
            }
            throw new System.InvalidOperationException(string.Format("Не удалось найти конечную точку с именем \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return OrderServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IOrderService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return OrderServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IOrderService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IOrderService,
        }
    }
}
