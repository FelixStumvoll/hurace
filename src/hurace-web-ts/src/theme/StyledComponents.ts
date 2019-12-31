import styled from 'styled-components';

export const Card = styled.div`
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    border: 1px solid rgba(128,128,128,0.5);
    border-radius: 10px;
`;

export const ItemCard = styled(Card)`
    margin-left: 10px;
    cursor: pointer;
    padding: 25px;

    :first-child {
        margin-left: 0px;
    }
`;
