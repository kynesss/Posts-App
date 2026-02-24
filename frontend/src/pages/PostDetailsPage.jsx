import usePost from "../hooks/usePost";
import { useParams, Link } from "react-router-dom";
import {
  Box,
  Card,
  CardContent,
  Typography,
  Stack,
  Button,
  Alert,
  CircularProgress,
} from "@mui/material";

const PostDetailsPage = () => {
  const { id } = useParams();
  const { post, isLoading, error } = usePost(id);

  return (
    <Box sx={{ maxWidth: 900, mx: "auto" }}>
      {error ? (
        <Alert severity="error">{error}</Alert>
      ) : isLoading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 4 }}>
          <CircularProgress size={56} color="secondary" />
        </Box>
      ) : (
        <Stack spacing={2}>
          <Button
            variant="contained"
            component={Link}
            to="/posts"
            size="medium"
            sx={{ width: 200 }}
          >
            Wróć do postów
          </Button>
          <Card>
            <CardContent>
              <Stack spacing={2} sx={{ width: "100%", maxWidth: 900 }}>
                <Typography variant="h5">{post.title}</Typography>
                <Typography variant="body1">{post.content}</Typography>
                <Typography variant="body2" color="textSecondary">
                  <b>Id:</b> {post.id} <b>CreatedAt:</b>{" "}
                  {new Date(post.createdAt).toLocaleString()}
                </Typography>
              </Stack>
            </CardContent>
          </Card>
        </Stack>
      )}
    </Box>
  );
};

export default PostDetailsPage;
