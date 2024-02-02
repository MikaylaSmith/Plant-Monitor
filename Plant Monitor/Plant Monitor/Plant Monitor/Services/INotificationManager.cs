/***********
* Interface: INotificationManager
*
* Purpose:
*	The purpose of this interface is to manage sending out notifications to the 
*	different operating systems so that they get handled properly on either Android or iOS
*
* Manager Functions:
*	None
*
*		
* Methods:
*	Initialize()
*		Base function to be overridden by system code.
*	SendNotification(string, string, DateTime?)
*		Base function to be overridden by system code. 
*	ReceiveNotification(string, string)
*		Base function to be overridden by system code
*	
*
***********/
using System;
using System.Collections.Generic;
using System.Text;

namespace Plant_Monitor.Services
{
	/* Purpose: Display filtered search results based on the entered characters and the radio button selected 
	 * Input: object, TextChangedEventArgs
	 * Output: Filtered list based on search results. 
	 */
	public interface INotificationManager
	{
		event EventHandler NotificationReceived;
		void Initialize();
		void SendNotification(string title, string message, DateTime? notifyTime = null);
		void ReceiveNotification(string title, string message);
	}
}
