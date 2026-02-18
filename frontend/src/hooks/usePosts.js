import { getPosts } from "../api/postsApi";
import { useEffect, useState } from "react";

const usePosts = (pager) => {
  const [posts, setPosts] = useState({ items: [], totalPages: 1, page: 1 });
  const [isLoading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getPosts(pager);
        setLoading(true);
        setPosts(data);
        setError(null);
      } catch (err) {
        setError(err?.message || "Failed to fetch posts");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [pager]);

  return { posts, isLoading, error };
};

export default usePosts;
