using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class ChefProfilesService : IChefProfilesService
    {
        IDataProvider _data = null;
        IUserMapper _mapper = null;

        public ChefProfilesService(IDataProvider data, IUserMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        
        public void Delete(int id)
        { 
            _data.ExecuteNonQuery("dbo.ChefProfiles_DeleteById"
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                });
        }
        public void Update(ChefProfileUpdateRequest model, int userId)
        {
            _data.ExecuteNonQuery("dbo.ChefProfiles_Update"
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col, userId);
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@StatusId", model.StatusId);

                }, returnParameters: null);
        }
        public int Add(ChefProfileAddRequest model, int userId)
        {
            int id = 0;

            _data.ExecuteNonQuery("dbo.ChefProfiles_Insert"
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col, userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                }

                , returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);
                });

            return id;
        }

        public ChefProfile Get(int id)
        {
            ChefProfile chefProfile = null;

            _data.ExecuteCmd("dbo.ChefProfiles_SelectById"
                ,inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                },singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    if (chefProfile == null)
                    {
                        chefProfile = MapChefProfile(reader, out int startingIndex);
                    }
                });

            return chefProfile;
        }

        public Paged<ChefProfile> SearchChef(string query, int pageIndex, int pageSize)
        {
            Paged<ChefProfile> pagedList = null;
            List<ChefProfile> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.ChefProfiles_SearchWithPagination"
                ,inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@query", query);
                    col.AddWithValue("@pageIndex", pageIndex);
                    col.AddWithValue("@pageSize", pageSize);

                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ChefProfile chefProfile = MapChefProfile(reader, out int startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<ChefProfile>();
                    }

                    list.Add(chefProfile);

                }, returnParameters: null);

            if (list != null)
            {
                pagedList = new Paged<ChefProfile>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<ChefProfile> GetAll(int pageIndex, int pageSize)
        {
            Paged<ChefProfile> pagedList = null;
            List<ChefProfile> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.ChefProfiles_SelectAll"
                ,inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@pageIndex", pageIndex);
                    col.AddWithValue("@pageSize", pageSize);

                },singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ChefProfile chefProfile = MapChefProfile(reader, out int startingIndex);

                    if(totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<ChefProfile>();
                    }
                    
                    list.Add(chefProfile);

                },returnParameters: null);

            if(list != null)
            {
                pagedList = new Paged<ChefProfile>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<ChefProfile> GetCurrentUser(int userId, int pageIndex, int pageSize)
        {
            Paged<ChefProfile> pagedList = null;
            List<ChefProfile> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.ChefProfiles_SelectByCreatedBy"
                ,inputParamMapper: delegate (SqlParameterCollection col)
                {
                     col.AddWithValue("@UserId", userId);
                     col.AddWithValue("@pageIndex", pageIndex);
                     col.AddWithValue("@pageSize", pageSize);

                },singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ChefProfile chefProfile = MapChefProfile(reader, out int startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<ChefProfile>();
                    }
                    
                    list.Add(chefProfile);

                },returnParameters: null);

            if (list != null)
            {
                pagedList = new Paged<ChefProfile>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;

        }

        private ChefProfile MapChefProfile(IDataReader reader, out int startingIndex)
        {
            ChefProfile chefProfile = new ChefProfile();

            startingIndex = 0;

            chefProfile.Id = reader.GetSafeInt32(startingIndex++);
            chefProfile.Bio = reader.GetSafeString(startingIndex++);
            chefProfile.AvatarThumbnailUrl = reader.GetSafeString(startingIndex++);
            chefProfile.AvatarUrl = reader.GetSafeString(startingIndex++);
            chefProfile.StatusId = reader.GetSafeString(startingIndex++);
            chefProfile.DateCreated = reader.GetSafeDateTime(startingIndex++);
            chefProfile.DateModified = reader.GetSafeDateTime(startingIndex++);

            chefProfile.CreatedBy = _mapper.MapUser(reader, ref startingIndex);

            string lanuagesAsString = reader.GetSafeString(startingIndex++);

            if (!string.IsNullOrEmpty(lanuagesAsString))
            {
                chefProfile.Languages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LookUp>>(lanuagesAsString);
            }


            return chefProfile;
        }


        private static void AddCommonParams(ChefProfileAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@AvatarThumbnailUrl", model.AvatarThumbnailUrl);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
            col.AddWithValue("@UserId", userId);
        }

    }
}

