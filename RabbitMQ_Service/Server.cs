using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using console_service.Patterns.Command;
using Newtonsoft.Json;

namespace console_service.RabbitMQ_Service
{
    public class Server : IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private string _queueName;
        private ICommand _command;

        public Server(string name, ICommand command) {
            this._queueName = name;
            this._command = command;
        }

        public void connect() {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            // Open the connection and create the channel
            this._connection = factory.CreateConnection();
            this._channel = this._connection.CreateModel();

            // Declare the queue
            this._channel.QueueDeclare(this._queueName, false, false, false, null);
            
            // Set the quality of Service
            this._channel.BasicQos(0, 1, false);

            this._consumer = new EventingBasicConsumer(this._channel);
            this._channel.BasicConsume(this._queueName, false, this._consumer);

            Console.WriteLine("Waiting for RPC requests ");

            this._consumer.Received += receiveRequest;
        }

        private void receiveRequest(Object mode, BasicDeliverEventArgs args)
        {

            var body = args.Body.ToArray();
            Console.WriteLine($"Body: {args.Body}");
            var props = args.BasicProperties;
            var replyProps = this._channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            string data = "";
            object responseObject = null;

            Exception ex = null;

            try
            {
                //data = Encoding.UTF8.GetString(body); // Encoding.UTF8.GetString(body);
                responseObject = this._command.Call(body);
                // response = ObjectSerialize.Serialize(responseObject);

            }
            catch (Exception exy)
            {
                ex = exy;
                Console.WriteLine(ex.Message);
            } 
            finally
            {
                if (ex != null) {
                    responseObject = ex.Message.Serialize();
                }

                // Console.WriteLine(Encoding.UTF8.GetString(responseObject as byte[]));
                this._channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseObject as byte[]);
                this._channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            }
        }


        public void Dispose() {

        }
    }
}