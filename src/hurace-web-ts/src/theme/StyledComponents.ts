import styled from 'styled-components';
import { Link } from 'react-router-dom';

export const Card = styled.div`
    /* box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2); */
    border: 1px solid rgba(128, 128, 128, 0.5);
    border-radius: 5px;
    padding: 10px;
`;

export const ListItem = styled.div`
    border-bottom: 1px solid rgba(128, 128, 128, 0.5);
    display: flex;
    :last-child {
        border-bottom: none;
    }
`;

export const DefaultLink = styled(Link)`
    text-decoration: none;
    color: black;
`;

export const DefaultInput = styled.input`
    border-radius: 5px;
    padding-left: 10px;
    border: 1px solid ${props => props.theme.gray};
    
`;
