﻿using System;
using Autofac;
using RabbitMQ.Client;

namespace RabbitMQSkeleton
{
    public class RabbitMQSkeletonModule : Module
    {
        private readonly string _uri;

        public RabbitMQSkeletonModule(string uri)
        {
            _uri = uri;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                Uri = _uri
            }).SingleInstance();


            builder.Register(c =>
            {
                var connectionFactory = c.Resolve<ConnectionFactory>();
                return connectionFactory.CreateConnection();
            }).SingleInstance();



            builder.Register(c => c.Resolve<IConnection>().CreateModel())
                .As<IModel>()
                .InstancePerDependency()
                .OnActivated(e => {Console.WriteLine("Create a new channel");})
                .OnRelease(e => {Console.WriteLine("Releasign the channel ");});

            builder.RegisterGeneric(typeof(ScheletonSetup<>));

            builder.RegisterGeneric( typeof(DefaultConsumer<>));

            builder.RegisterGeneric(typeof(NewSetupper<>));
            builder.RegisterGeneric(typeof(NewSetupper2<,>));

        }
    }
}