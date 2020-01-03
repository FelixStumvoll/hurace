import React from 'react';
import { Skier } from '../../interfaces/Skier';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { Card } from '../../theme/StyledComponents';

const SkierLink = styled(Link)`
    text-decoration: none;
    color: black;
`;

const SkierCard = styled(Card)`
    padding: 0px;
    width: 200px;
    height: 300px;
    margin: 10px 10px 0 0;
    display: grid;
    grid-template-rows: 200px 1fr;
    flex-direction: column;
`;

const ImagePanel = styled.div`
    display: flex;
    justify-content: center;
`;

const SkierImage = styled.img`
    object-fit: cover;
    height: 100%;
    width: 100%;
`;

const SkierCardContent = styled.div`
    padding: 10px;
    display: flex;
    flex-direction: column;
`;

const SkierName = styled.div`
    font-weight: bold;
    margin-top: auto;
`;

const SkierCountry = styled.div`
    font-style: italic;
    color: gray;
`;

export const SkierListItem: React.FC<{ skier: Skier }> = ({ skier }) => {
    return (
        <SkierLink to={`/skier/${skier.id}`}>
            <SkierCard>
                {skier.imageUrl && (
                    <ImagePanel>
                        <SkierImage alt="Skier" src={skier.imageUrl} />
                    </ImagePanel>
                )}
                <SkierCardContent>
                    <SkierName>
                        {skier.firstName} {skier.lastName}
                    </SkierName>
                    <SkierCountry>{skier.country.countryName}</SkierCountry>
                </SkierCardContent>
            </SkierCard>
        </SkierLink>
    );
};
