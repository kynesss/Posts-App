import axios from "axios";

const POSTS_URL = "http://localhost:8080/posts";

export const getPosts = async (pager) => {
  const response = await axios.get(POSTS_URL, {
    params: pager,
  });
  return response.data;
};
