using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace XboxController
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.tempuri.org/2012/04/xboxcontroller.html";
	}
	
	[DataContract]
	public class XboxControllerState
	{
	}
	
	[ServicePort]
	public class XboxControllerOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<XboxControllerState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<XboxControllerState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}
}


