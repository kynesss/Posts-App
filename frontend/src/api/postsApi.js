import axios from "axios";

const POSTS_URL = "http://localhost:8080/posts";

export const getPosts = async (search, pager) => {
  const response = await axios.get(POSTS_URL, {
    params: {
      search,
      ...pager,
    },
  });
  return response.data;
};

export const fetchPost = async (id) => {
  return (await axios.get(`${POSTS_URL}/${id}`)).data;
};

export const addPost = async (post) => {
  return (await axios.post(POSTS_URL, post)).data;
};

export const removePost = async (id) => {
  return (await axios.delete(`${POSTS_URL}/${id}`)).data;
};

export const editPost = async (id, post) => {
  return await axios.put(`${POSTS_URL}/${id}`, post);
};
