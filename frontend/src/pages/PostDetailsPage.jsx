import usePost from "../hooks/usePost";
import { useParams } from "react-router-dom";

const PostDetailsPage = () => {
  const { id } = useParams();
  const { post, isLoading, error } = usePost(id);

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return <div>{post.title}</div>;
};

export default PostDetailsPage;
