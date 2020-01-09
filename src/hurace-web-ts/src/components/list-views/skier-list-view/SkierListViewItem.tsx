import React from 'react';
import { Skier } from '../../../models/Skier';
import styled from 'styled-components';
import { Card, DefaultLink } from '../../../theme/CustomComponents';

const SkierCard = styled(Card)`
    padding: 0px;
    width: 200px;
    height: 300px;
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

const NoImage = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
    border-bottom: 1px solid ${props => props.theme.gray};
`;

const SkierCardContent = styled.div`
    padding: ${props => props.theme.gap};
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

export const SkierListViewItem: React.FC<{ skier: Skier }> = ({ skier }) => {
    return (
        <DefaultLink to={`/skier/${skier.id}`}>
            <SkierCard>
                {skier.imageUrl ? (
                    <ImagePanel>
                        <SkierImage alt="Skier" src={skier.imageUrl} />
                    </ImagePanel>
                ) : (
                    <NoImage>Kein Bild verfügbar</NoImage>
                )}
                <SkierCardContent>
                    <SkierName>
                        {skier.firstName} {skier.lastName}
                    </SkierName>
                    <SkierCountry>{skier.country?.countryName}</SkierCountry>
                </SkierCardContent>
            </SkierCard>
        </DefaultLink>
    );
};
