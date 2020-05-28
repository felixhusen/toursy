using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using localtour.Authorization.Users;
using localtour.Helpers;
using localtour.Messages.Dto;
using localtour.States;
using localtour.Tours;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Messages
{

    public class MessageAppService : localtourAppServiceBase, IMessageAppService
    {
        private readonly IRepository<Message, int> _messageRepository;
        private readonly IRepository<Tour, int> _tourRepository;
        private readonly IRepository<State, int> _stateRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MessageAppService(IRepository<Message, int> messageRepository, IRepository<Tour, int> tourRepository, IRepository<State, int> stateRepository, IRepository<User, long> userRepository, IWebHostEnvironment hostEnvironment)
        {
            _messageRepository = messageRepository;
            _tourRepository = tourRepository;
            _stateRepository = stateRepository;
            _userRepository = userRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetMessageForViewDto>> GetMessages()
        {
            try
            {
                var filteredMessages = _messageRepository.GetAll().Where(e => e.ReceiverId == AbpSession.UserId || e.SenderId == AbpSession.UserId);

                var messages = from o in filteredMessages

                               join receiver in _userRepository.GetAll() on o.ReceiverId equals receiver.Id
                               join sender in _userRepository.GetAll() on o.SenderId equals sender.Id

                               select new GetMessageForViewDto()
                               {
                                   Message = new MessageDto()
                                   {
                                       Id = o.Id,
                                       SenderId = o.SenderId,
                                       Content = o.Content,
                                       DateSent = o.DateSent,
                                       ReceiverId = o.ReceiverId
                                   },
                                   DisplayName = receiver.Id == AbpSession.UserId ? sender.FullName : receiver.FullName,
                                   RelatedUserId = receiver.Id == AbpSession.UserId ? sender.Id : receiver.Id
                               };

                var results = messages.OrderBy("Message.DateSent desc").DistinctBy(e => e.DisplayName);

                var totalCount = results.Count();

                return new PagedResultDto<GetMessageForViewDto>(
                    totalCount,
                    results.ToList()
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<GetMessageForViewDto> SendMessage(CreateMessageDto input)
        {
            var message = new Message
            {
                Content = input.Content,
                DateSent = DateTime.Now,
                ReceiverId = input.ReceiverId,
                SenderId = AbpSession.UserId
            };

            var id = await _messageRepository.InsertAndGetIdAsync(message);

            var receiver = await _userRepository.GetAsync((long)input.ReceiverId);

            var sender = await _userRepository.GetAsync((long)AbpSession.UserId);

            var output = new GetMessageForViewDto
            {
                Message = ObjectMapper.Map<MessageDto>(message),
                DisplayName = receiver.Id == AbpSession.UserId ? sender.FullName : receiver.FullName,
                RelatedUserId = receiver.Id == AbpSession.UserId ? sender.Id : receiver.Id
            };

            return output;
        }

        public async Task<List<GetMessageForViewDto>> GetMessagesByRelatedUserId(int UserId)
        {
            var filteredMessages = _messageRepository.GetAll().Where(e => (e.SenderId == UserId && e.ReceiverId == AbpSession.UserId) || (e.SenderId == AbpSession.UserId && e.ReceiverId == UserId));

            var messages = from o in filteredMessages

                           join receiver in _userRepository.GetAll() on o.ReceiverId equals receiver.Id
                           join sender in _userRepository.GetAll() on o.SenderId equals sender.Id

                           select new GetMessageForViewDto()
                           {
                               Message = new MessageDto()
                               {
                                   Id = o.Id,
                                   SenderId = o.SenderId,
                                   Content = o.Content,
                                   DateSent = o.DateSent,
                                   ReceiverId = o.ReceiverId
                               },
                               DisplayName = receiver.Id == AbpSession.UserId ? sender.FullName : receiver.FullName,
                               RelatedUserId = receiver.Id == AbpSession.UserId ? sender.Id : receiver.Id
                           };

            var pagedAndFilteredMessages = messages
                .OrderBy("Message.DateSent desc");

            return await pagedAndFilteredMessages.ToListAsync();
        }

    }
}
