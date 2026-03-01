import axios from "axios";

const BASE_URL = "http://localhost:8080";

export const getComments = async (postId, search, pager) => {
  const response = await axios.get(`${BASE_URL}/posts/${postId}/comments`, {
    params: {
      search,
      ...pager,
    },
  });
  return response.data;
};

export const addComment = async (postId, comment) => {
  return (await axios.post(`${BASE_URL}/posts/${postId}/comments`, comment))
    .data;
};

export const editComment = async (id, comment) => {
  return (await axios.put(`${BASE_URL}/comments/${id}`, comment)).data;
};

export const deleteComment = async (id) => {
  return (await axios.delete(`${BASE_URL}/comments/${id}`)).data;
};
