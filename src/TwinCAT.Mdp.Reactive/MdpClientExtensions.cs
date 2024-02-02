using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwinCAT.Ads;
using TwinCAT.Ads.Internal;
using TwinCAT.Mdp.Abstractions;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Reactive
{
	/// <summary>
	/// Provides extension methods for the <see cref="IMdpConnection"/> interface.
	/// </summary>
	public static class MdpClientExtensions
	{
		/// <summary>
		/// Returns an observable sequence of connection state changes.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <returns>An observable sequence of connection state changes.</returns>
		public static IObservable<ConnectionState> WhenConnectionStateChanges(
			this IMdpConnection connection
		)
		{
			return Observable.Select(
				Observable.FromEventPattern<
					EventHandler<ConnectionStateChangedEventArgs>,
					ConnectionStateChangedEventArgs
				>(
					delegate(EventHandler<ConnectionStateChangedEventArgs> h)
					{
						connection.Connection.ConnectionStateChanged += h;
					},
					delegate(EventHandler<ConnectionStateChangedEventArgs> h)
					{
						connection.Connection.ConnectionStateChanged -= h;
					}
				),
				(EventPattern<ConnectionStateChangedEventArgs> ev) => ev.EventArgs.NewState
			);
		}

		/// <summary>
		/// Returns an observable sequence of value changes for the specified MDP parameter.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type of the parameter.</param>
		/// <returns>An observable sequence of value changes for the specified MDP parameter.</returns>
		public static IObservable<object> WhenValueChanged(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type
		)
		{
			return connection.WhenValueChanged(
				moduleType,
				tableID,
				subIndex,
				type,
				NotificationSettings.Default
			);
		}

		/// <summary>
		/// Returns an observable sequence of value changes for the specified MDP parameter.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <returns>An observable sequence of value changes for the specified MDP parameter.</returns>
		public static IObservable<T> WhenValueChanged<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex
		)
		{
			return connection.WhenValueChanged<T>(
				moduleType,
				tableID,
				subIndex,
				NotificationSettings.Default
			);
		}

		/// <summary>
		/// Returns an observable sequence of value changes for the specified MDP parameter with custom settings.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="settings">Notification settings for the observable.</param>
		/// <returns>An observable sequence of value changes for the specified MDP parameter.</returns>
		public static IObservable<T> WhenValueChanged<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			NotificationSettings settings
		)
		{
			return (IObservable<T>)
				connection.WhenValueChanged(moduleType, tableID, subIndex, typeof(T), settings);
		}

		/// <summary>
		/// Returns an observable sequence of value changes for the specified MDP parameter with custom settings.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type of the parameter.</param>
		/// <param name="settings">Notification settings for the observable.</param>
		/// <returns>An observable sequence of value changes for the specified MDP parameter.</returns>
		public static IObservable<object> WhenValueChanged(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			NotificationSettings settings
		)
		{
			return connection
				.PollParameter(
					moduleType,
					tableID,
					subIndex,
					type,
					TimeSpan.FromMilliseconds(settings.CycleTime)
				)
				.DistinctUntilChanged();
		}

		/// <summary>
		/// Polls a parameter from the specified MDP address triggered by the specified observable.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type of the parameter.</param>
		/// <param name="trigger">The observable trigger for polling.</param>
		/// <returns>An observable sequence of polled parameter values.</returns>
		public static IObservable<object> PollParameter(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			IObservable<Unit> trigger
		)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}

			Func<Unit, object> func = delegate(Unit o)
			{
				return connection.ReadParameter(moduleType, tableID, subIndex, type);
			};

			return Observable.Select(trigger, func);
		}

		/// <summary>
		/// Polls a parameter from the specified MDP address with a specified polling period.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type of the parameter.</param>
		/// <param name="period">The polling period.</param>
		/// <returns>An observable sequence of polled parameter values.</returns>
		public static IObservable<object> PollParameter(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			TimeSpan period
		)
		{
			return connection.PollParameter(
				moduleType,
				tableID,
				subIndex,
				type,
				Observable.Select<long, Unit>(
					Observable.StartWith<long>(Observable.Interval(period), new long[] { }),
					(long e) => Unit.Default
				)
			);
		}

		/// <summary>
		/// Polls a parameter of type <typeparamref name="T"/> from the specified MDP address triggered by the specified observable.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="trigger">The observable trigger for polling.</param>
		/// <returns>An observable sequence of polled parameter values of type <typeparamref name="T"/>.</returns>
		public static IObservable<T> PollParameter<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			IObservable<Unit> trigger
		)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}

			Func<Unit, T> func = delegate(Unit o)
			{
				return connection.ReadParameter<T>(moduleType, tableID, subIndex);
			};

			return Observable.Select(trigger, func);
		}

		/// <summary>
		/// Polls a parameter of type <typeparamref name="T"/> from the specified MDP address with a specified polling period.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="period">The polling period.</param>
		/// <returns>An observable sequence of polled parameter values of type <typeparamref name="T"/>.</returns>
		public static IObservable<T> PollParameter<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			TimeSpan period
		)
		{
			return (IObservable<T>)
				connection.PollParameter(moduleType, tableID, subIndex, typeof(T), period);
		}

		/// <summary>
		/// Polls a parameter of type <typeparamref name="T"/> from the specified MDP address with a specified polling period and annotates the result with parameter change information.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="period">The polling period.</param>
		/// <returns>An observable sequence of annotated parameter change events.</returns>
		public static IObservable<ParameterChangedEventArgs> PollParameterAnnotated<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			TimeSpan period
		)
		{
			return Observable.Select<T, ParameterChangedEventArgs>(
				connection.PollParameter<T>(moduleType, tableID, subIndex, period),
				(T o) =>
					new ParameterChangedEventArgs(
						new MdpAddress
						{
							Area = MdpArea.Config,
							ModuleID = connection.GetModuleID(moduleType, 1),
							Flag = 0,
							TableID = tableID,
							SubIndex = subIndex
						},
						DateTime.Now
					)
			);
		}

		/// <summary>
		/// Polls a parameter from the specified MDP address asynchronously triggered by the specified observable.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type of the parameter.</param>
		/// <param name="trigger">The observable trigger for polling.</param>
		/// <param name="cancel">Cancellation token.</param>
		/// <returns>An observable sequence of polled parameter values.</returns>
		public static IObservable<object> PollParameterAsync(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			IObservable<Unit> trigger,
			CancellationToken cancel
		)
		{
			Func<Unit, CancellationToken, Task<object>> func = async delegate(
				Unit o,
				CancellationToken cancellation
			)
			{
				return await connection.ReadParameterAsync(
					moduleType,
					tableID,
					subIndex,
					type,
					cancel
				);
			};

			return Observable.SelectMany(trigger, func);
		}

		/// <summary>
		/// Polls a parameter from the specified MDP address asynchronously with a specified polling period and cancellation token.
		/// </summary>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type of the parameter.</param>
		/// <param name="period">The polling period.</param>
		/// <param name="cancel">Cancellation token.</param>
		/// <returns>An observable sequence of polled parameter values.</returns>
		public static IObservable<object> PollParameterAsync(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			TimeSpan period,
			CancellationToken cancel
		)
		{
			return connection.PollParameterAsync(
				moduleType,
				tableID,
				subIndex,
				type,
				Observable.Select<long, Unit>(
					Observable.StartWith<long>(Observable.Interval(period), new long[] { }),
					(long e) => Unit.Default
				),
				cancel
			);
		}

		/// <summary>
		/// Polls a parameter of type <typeparamref name="T"/> from the specified MDP address asynchronously triggered by the specified observable.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="trigger">The observable trigger for polling.</param>
		/// <param name="cancel">Cancellation token.</param>
		/// <returns>An observable sequence of polled parameter values of type <typeparamref name="T"/>.</returns>
		public static IObservable<T> PollParameterAsync<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			IObservable<Unit> trigger,
			CancellationToken cancel
		)
		{
			Func<Unit, CancellationToken, Task<T>> func = async delegate(
				Unit o,
				CancellationToken cancellation
			)
			{
				return await connection.ReadParameterAsync<T>(
					moduleType,
					tableID,
					subIndex,
					cancel
				);
			};

			return Observable.SelectMany(trigger, func);
		}

		/// <summary>
		/// Polls a parameter of type <typeparamref name="T"/> from the specified MDP address asynchronously with a specified polling period and cancellation token.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="connection">The MDP connection.</param>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="period">The polling period.</param>
		/// <param name="cancel">Cancellation token.</param>
		/// <returns>An observable sequence of polled parameter values of type <typeparamref name="T"/>.</returns>
		public static IObservable<T> PollParameterAsync<T>(
			this IMdpConnection connection,
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			TimeSpan period,
			CancellationToken cancel
		)
		{
			return (IObservable<T>)
				connection.PollParameterAsync(
					moduleType,
					tableID,
					subIndex,
					typeof(T),
					period,
					cancel
				);
		}
	}
}
