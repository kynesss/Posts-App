import { fetchPost } from "../api/postsApi";
import { useEffect, useState } from "react";

const usePost = (id) => {
  const [post, setPost] = useState(null);
  const [isLoading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await fetchPost(id);
        setLoading(true);
        setPost(data);
        setError(null);
      } catch (err) {
        setError(err?.message || "Failed to fetch post");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  return { post, isLoading, error };
};

export default usePost;
