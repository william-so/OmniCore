﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniCore.Model;
using OmniCore.Model.Interfaces;
using OmniCore.Radio.RileyLink;

namespace OmniCore.Impl.Eros
{
    public class ErosPodProvider : IPodProvider
    {
        private IRadioProvider[] _radioProviders;
        private IPodRepository _podRepository;
        private IRadioAdapter _radioAdapter;

        public ErosPodProvider(IRadioAdapter radioAdapter,
            IRadioProvider[] radioProviders, 
            IPodRepository podRepository)
        {
            _radioProviders = radioProviders;
            _radioAdapter = radioAdapter;
            _podRepository = podRepository;
        }

        public async Task<IPod> GetActivePod()
        {
            var pods = await _podRepository.GetActivePods<ErosPod>();
            return pods.OrderByDescending(p => p.Created).FirstOrDefault();
        }

        public async Task<IEnumerable<IPod>> GetActivePods()
        {
            return (await _podRepository.GetActivePods<ErosPod>())
                .OrderBy(p => p.Created);
        }

        public async Task Archive(IPod pod)
        {
            var erosPod = pod as ErosPod;
            erosPod.Archived = true;
            await _podRepository.SavePod<ErosPod>(erosPod);
        }

        public async Task<IPod> New(IEnumerable<IRadio> radios)
        {
            var pod = new ErosPod
            {
                Id = Guid.NewGuid(),
                ProviderSpecificRadioIds = radios.Select(r => r.ProviderSpecificId).ToArray(),
                RadioAddress = GenerateRadioAddress()
            };
            await _podRepository.SavePod<ErosPod>(pod);
            return pod;
        }

        public async Task<IPod> Register(uint lot, uint serial, uint radioAddress, IEnumerable<IRadio> radios)
        {
            var pod = new ErosPod
            {
                Id = Guid.NewGuid(),
                Lot = lot,
                Serial = serial,
                RadioAddress = radioAddress,
                ProviderSpecificRadioIds = radios.Select(r => r.ProviderSpecificId).ToArray(),
            };
            await _podRepository.SavePod<ErosPod>(pod);
            return pod;
        }

        public Task CancelConversations(IPod pod)
        {
            throw new NotImplementedException();
        }

        public IObservable<IRadio> ListAllRadios()
        {
            return Observable.Create<IRadio>((IObserver<IRadio> observer) =>
            {
                var disposables = new List<IDisposable>();
                foreach (var radioProvider in _radioProviders)
                {
                    disposables.Add(radioProvider.ListRadios()
                        .Subscribe(radio =>
                        {
                            observer.OnNext(radio);
                        }));
                }

                return Disposable.Create(() =>
                {
                    foreach(var disposable in disposables)
                        disposable.Dispose();

                });
            });
        }

        private uint GenerateRadioAddress()
        {
            var random = new Random();
            var buffer = new byte[3];
            random.NextBytes(buffer);
            uint address = 0x34000000;
            address |= (uint)buffer[0] << 16;
            address |= (uint)buffer[1] << 8;
            address |= (uint)buffer[2];
            return address;
        }
    }
}
