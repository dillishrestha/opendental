﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace OpenDental.MobileWeb {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
	using OpenDentBusiness;
	using OpenDentBusiness.Mobile;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MobileSoap", Namespace="http://opendental.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TableBase))]
    public partial class Mobile : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ServiceExistsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCustomerNumOperationCompleted;
        
        private System.Threading.SendOrPostCallback IsPaidCustomerOperationCompleted;
        
        private System.Threading.SendOrPostCallback SynchPatientsOperationCompleted;
        
        private System.Threading.SendOrPostCallback SynchAppointmentsOperationCompleted;
        
        private System.Threading.SendOrPostCallback SynchPrescriptionsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetUserNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetMobileWebUserPasswordOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Mobile() {
            this.Url = global::OpenDental.Properties.Settings.Default.OpenDental_MobileWeb_Mobile;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ServiceExistsCompletedEventHandler ServiceExistsCompleted;
        
        /// <remarks/>
        public event GetCustomerNumCompletedEventHandler GetCustomerNumCompleted;
        
        /// <remarks/>
        public event IsPaidCustomerCompletedEventHandler IsPaidCustomerCompleted;
        
        /// <remarks/>
        public event SynchPatientsCompletedEventHandler SynchPatientsCompleted;
        
        /// <remarks/>
        public event SynchAppointmentsCompletedEventHandler SynchAppointmentsCompleted;
        
        /// <remarks/>
        public event SynchPrescriptionsCompletedEventHandler SynchPrescriptionsCompleted;
        
        /// <remarks/>
        public event GetUserNameCompletedEventHandler GetUserNameCompleted;
        
        /// <remarks/>
        public event SetMobileWebUserPasswordCompletedEventHandler SetMobileWebUserPasswordCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/ServiceExists", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ServiceExists() {
            object[] results = this.Invoke("ServiceExists", new object[0]);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ServiceExistsAsync() {
            this.ServiceExistsAsync(null);
        }
        
        /// <remarks/>
        public void ServiceExistsAsync(object userState) {
            if ((this.ServiceExistsOperationCompleted == null)) {
                this.ServiceExistsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServiceExistsOperationCompleted);
            }
            this.InvokeAsync("ServiceExists", new object[0], this.ServiceExistsOperationCompleted, userState);
        }
        
        private void OnServiceExistsOperationCompleted(object arg) {
            if ((this.ServiceExistsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServiceExistsCompleted(this, new ServiceExistsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/GetCustomerNum", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long GetCustomerNum(string RegistrationKeyFromDentalOffice) {
            object[] results = this.Invoke("GetCustomerNum", new object[] {
                        RegistrationKeyFromDentalOffice});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void GetCustomerNumAsync(string RegistrationKeyFromDentalOffice) {
            this.GetCustomerNumAsync(RegistrationKeyFromDentalOffice, null);
        }
        
        /// <remarks/>
        public void GetCustomerNumAsync(string RegistrationKeyFromDentalOffice, object userState) {
            if ((this.GetCustomerNumOperationCompleted == null)) {
                this.GetCustomerNumOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCustomerNumOperationCompleted);
            }
            this.InvokeAsync("GetCustomerNum", new object[] {
                        RegistrationKeyFromDentalOffice}, this.GetCustomerNumOperationCompleted, userState);
        }
        
        private void OnGetCustomerNumOperationCompleted(object arg) {
            if ((this.GetCustomerNumCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCustomerNumCompleted(this, new GetCustomerNumCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/IsPaidCustomer", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool IsPaidCustomer(string RegistrationKey) {
            object[] results = this.Invoke("IsPaidCustomer", new object[] {
                        RegistrationKey});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void IsPaidCustomerAsync(string RegistrationKey) {
            this.IsPaidCustomerAsync(RegistrationKey, null);
        }
        
        /// <remarks/>
        public void IsPaidCustomerAsync(string RegistrationKey, object userState) {
            if ((this.IsPaidCustomerOperationCompleted == null)) {
                this.IsPaidCustomerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnIsPaidCustomerOperationCompleted);
            }
            this.InvokeAsync("IsPaidCustomer", new object[] {
                        RegistrationKey}, this.IsPaidCustomerOperationCompleted, userState);
        }
        
        private void OnIsPaidCustomerOperationCompleted(object arg) {
            if ((this.IsPaidCustomerCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.IsPaidCustomerCompleted(this, new IsPaidCustomerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/SynchPatients", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SynchPatients(string RegistrationKey, Patientm[] patientmList) {
            this.Invoke("SynchPatients", new object[] {
                        RegistrationKey,
                        patientmList});
        }
        
        /// <remarks/>
        public void SynchPatientsAsync(string RegistrationKey, Patientm[] patientmList) {
            this.SynchPatientsAsync(RegistrationKey, patientmList, null);
        }
        
        /// <remarks/>
        public void SynchPatientsAsync(string RegistrationKey, Patientm[] patientmList, object userState) {
            if ((this.SynchPatientsOperationCompleted == null)) {
                this.SynchPatientsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSynchPatientsOperationCompleted);
            }
            this.InvokeAsync("SynchPatients", new object[] {
                        RegistrationKey,
                        patientmList}, this.SynchPatientsOperationCompleted, userState);
        }
        
        private void OnSynchPatientsOperationCompleted(object arg) {
            if ((this.SynchPatientsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SynchPatientsCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/SynchAppointments", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SynchAppointments(string RegistrationKey, Appointmentm[] appointmentList) {
            this.Invoke("SynchAppointments", new object[] {
                        RegistrationKey,
                        appointmentList});
        }
        
        /// <remarks/>
        public void SynchAppointmentsAsync(string RegistrationKey, Appointmentm[] appointmentList) {
            this.SynchAppointmentsAsync(RegistrationKey, appointmentList, null);
        }
        
        /// <remarks/>
        public void SynchAppointmentsAsync(string RegistrationKey, Appointmentm[] appointmentList, object userState) {
            if ((this.SynchAppointmentsOperationCompleted == null)) {
                this.SynchAppointmentsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSynchAppointmentsOperationCompleted);
            }
            this.InvokeAsync("SynchAppointments", new object[] {
                        RegistrationKey,
                        appointmentList}, this.SynchAppointmentsOperationCompleted, userState);
        }
        
        private void OnSynchAppointmentsOperationCompleted(object arg) {
            if ((this.SynchAppointmentsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SynchAppointmentsCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/SynchPrescriptions", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SynchPrescriptions(string RegistrationKey, RxPatm[] rxList) {
            this.Invoke("SynchPrescriptions", new object[] {
                        RegistrationKey,
                        rxList});
        }
        
        /// <remarks/>
        public void SynchPrescriptionsAsync(string RegistrationKey, RxPatm[] rxList) {
            this.SynchPrescriptionsAsync(RegistrationKey, rxList, null);
        }
        
        /// <remarks/>
        public void SynchPrescriptionsAsync(string RegistrationKey, RxPatm[] rxList, object userState) {
            if ((this.SynchPrescriptionsOperationCompleted == null)) {
                this.SynchPrescriptionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSynchPrescriptionsOperationCompleted);
            }
            this.InvokeAsync("SynchPrescriptions", new object[] {
                        RegistrationKey,
                        rxList}, this.SynchPrescriptionsOperationCompleted, userState);
        }
        
        private void OnSynchPrescriptionsOperationCompleted(object arg) {
            if ((this.SynchPrescriptionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SynchPrescriptionsCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/GetUserName", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetUserName(string RegistrationKey) {
            object[] results = this.Invoke("GetUserName", new object[] {
                        RegistrationKey});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetUserNameAsync(string RegistrationKey) {
            this.GetUserNameAsync(RegistrationKey, null);
        }
        
        /// <remarks/>
        public void GetUserNameAsync(string RegistrationKey, object userState) {
            if ((this.GetUserNameOperationCompleted == null)) {
                this.GetUserNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUserNameOperationCompleted);
            }
            this.InvokeAsync("GetUserName", new object[] {
                        RegistrationKey}, this.GetUserNameOperationCompleted, userState);
        }
        
        private void OnGetUserNameOperationCompleted(object arg) {
            if ((this.GetUserNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUserNameCompleted(this, new GetUserNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://opendental.com/SetMobileWebUserPassword", RequestNamespace="http://opendental.com/", ResponseNamespace="http://opendental.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SetMobileWebUserPassword(string RegistrationKey, string UserName, string Password) {
            this.Invoke("SetMobileWebUserPassword", new object[] {
                        RegistrationKey,
                        UserName,
                        Password});
        }
        
        /// <remarks/>
        public void SetMobileWebUserPasswordAsync(string RegistrationKey, string UserName, string Password) {
            this.SetMobileWebUserPasswordAsync(RegistrationKey, UserName, Password, null);
        }
        
        /// <remarks/>
        public void SetMobileWebUserPasswordAsync(string RegistrationKey, string UserName, string Password, object userState) {
            if ((this.SetMobileWebUserPasswordOperationCompleted == null)) {
                this.SetMobileWebUserPasswordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetMobileWebUserPasswordOperationCompleted);
            }
            this.InvokeAsync("SetMobileWebUserPassword", new object[] {
                        RegistrationKey,
                        UserName,
                        Password}, this.SetMobileWebUserPasswordOperationCompleted, userState);
        }
        
        private void OnSetMobileWebUserPasswordOperationCompleted(object arg) {
            if ((this.SetMobileWebUserPasswordCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetMobileWebUserPasswordCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ServiceExistsCompletedEventHandler(object sender, ServiceExistsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServiceExistsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ServiceExistsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetCustomerNumCompletedEventHandler(object sender, GetCustomerNumCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCustomerNumCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCustomerNumCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void IsPaidCustomerCompletedEventHandler(object sender, IsPaidCustomerCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IsPaidCustomerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal IsPaidCustomerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SynchPatientsCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SynchAppointmentsCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SynchPrescriptionsCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetUserNameCompletedEventHandler(object sender, GetUserNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUserNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetUserNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SetMobileWebUserPasswordCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591